using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MidDishApp.Data;
using MidDishApp.Models;

namespace MidDishApp.Controllers;

public class DishesController : BaseController
{
    private readonly AppDbContext _db;
    public DishesController(AppDbContext db) => _db = db;

    private async Task LoadCategories() =>
        ViewBag.Categories = await _db.DishCategories.AsNoTracking().OrderBy(c => c.Name).ToListAsync();

    // ---------- LIST: search + filter + price range + sort (all done in SQL) ----------
    public async Task<IActionResult> Index(DishFilter f)
    {
        var vm = new DishIndexVm
        {
            Filter = f,
            Categories = await _db.DishCategories.AsNoTracking().OrderBy(c => c.Name).ToListAsync()
        };

        // price inputs that are not numbers
        if ((ModelState.TryGetValue(nameof(f.MinPrice), out var a) && a.Errors.Count > 0) ||
            (ModelState.TryGetValue(nameof(f.MaxPrice), out var b) && b.Errors.Count > 0))
        {
            vm.RangeError = "Err.PriceInvalid";
            return View(vm);
        }

        // invalid range: negative, or from > to
        if (f.MinPrice < 0 || f.MaxPrice < 0 ||
            (f.MinPrice.HasValue && f.MaxPrice.HasValue && f.MinPrice > f.MaxPrice))
        {
            vm.RangeError = "Err.PriceRange";
            return View(vm);
        }

        IQueryable<Dish_BIT240009> query = _db.Dishes
            .AsNoTracking()
            .Include(d => d.DishCategory)
            .Include(d => d.Images);

        var q = f.Q?.Trim();
        if (!string.IsNullOrEmpty(q))
            query = query.Where(d => d.Name.Contains(q));

        if (f.CategoryId.HasValue)
            query = query.Where(d => d.DishCategoryId == f.CategoryId.Value);

        if (f.Status == "available")
            query = query.Where(d => d.IsAvailable);
        else if (f.Status == "unavailable")
            query = query.Where(d => !d.IsAvailable);

        if (f.MinPrice.HasValue)
            query = query.Where(d => d.Price >= f.MinPrice.Value);
        if (f.MaxPrice.HasValue)
            query = query.Where(d => d.Price <= f.MaxPrice.Value);

        query = f.Sort switch
        {
            "price_asc" => query.OrderBy(d => d.Price).ThenBy(d => d.Id),
            "price_desc" => query.OrderByDescending(d => d.Price).ThenBy(d => d.Id),
            "time_asc" => query.OrderBy(d => d.PreparationTime).ThenBy(d => d.Id),
            _ => query.OrderBy(d => d.Id)
        };

        vm.Dishes = await query.ToListAsync();
        return View(vm);
    }

    // ---------- DETAIL ----------
    public async Task<IActionResult> Detail(int? id)
    {
        if (id == null) return NotFoundPage("Err.DishNotFound");

        var dish = await _db.Dishes.AsNoTracking()
            .Include(d => d.DishCategory)
            .Include(d => d.Images)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (dish == null) return NotFoundPage("Err.DishNotFound");
        return View(dish);
    }

    // ---------- CREATE ----------
    public async Task<IActionResult> Create()
    {
        await LoadCategories();
        return View(new Dish_BIT240009 { IsAvailable = true, PreparationTime = 10 });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Name,Price,PreparationTime,IsAvailable,Description,DishCategoryId")] Dish_BIT240009 dish)
    {
        await ValidateDish(dish);
        if (!ModelState.IsValid)
        {
            await LoadCategories();
            return View(dish);
        }

        _db.Dishes.Add(dish);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Msg.Saved";
        return RedirectToAction(nameof(Index));
    }

    // ---------- EDIT ----------
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFoundPage("Err.DishNotFound");
        var dish = await _db.Dishes.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        if (dish == null) return NotFoundPage("Err.DishNotFound");

        await LoadCategories();
        return View(dish);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id,
        [Bind("Id,Name,Price,PreparationTime,IsAvailable,Description,DishCategoryId")] Dish_BIT240009 input)
    {
        if (id == null || id != input.Id) return NotFoundPage("Err.DishNotFound");

        var dish = await _db.Dishes.FirstOrDefaultAsync(d => d.Id == id);
        if (dish == null) return NotFoundPage("Err.DishNotFound");

        await ValidateDish(input);
        if (!ModelState.IsValid)
        {
            await LoadCategories();
            return View(input);
        }

        dish.Name = input.Name;
        dish.Price = input.Price;
        dish.PreparationTime = input.PreparationTime;
        dish.IsAvailable = input.IsAvailable;
        dish.Description = input.Description;
        dish.DishCategoryId = input.DishCategoryId;
        await _db.SaveChangesAsync();

        TempData["Success"] = "Msg.Saved";
        return RedirectToAction(nameof(Index));
    }

    // ---------- DELETE (dish; its images are deleted by cascade) ----------
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFoundPage("Err.DishNotFound");
        var dish = await _db.Dishes.FirstOrDefaultAsync(d => d.Id == id);
        if (dish == null) return NotFoundPage("Err.DishNotFound");

        _db.Dishes.Remove(dish);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Msg.Deleted";
        return RedirectToAction(nameof(Index));
    }

    // ---------- IMAGES ----------
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> AddImage(int dishId, string imageUrl, bool isThumbnail)
    {
        var dish = await _db.Dishes.Include(d => d.Images).FirstOrDefaultAsync(d => d.Id == dishId);
        if (dish == null) return NotFoundPage("Err.DishNotFound");

        imageUrl = imageUrl?.Trim();
        if (string.IsNullOrEmpty(imageUrl))
        {
            TempData["Error"] = "Val.UrlRequired";
            return RedirectToAction(nameof(Detail), new { id = dishId });
        }
        if (imageUrl.Length > 500 ||
            !Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            TempData["Error"] = "Val.UrlInvalid";
            return RedirectToAction(nameof(Detail), new { id = dishId });
        }

        // first image becomes the thumbnail automatically; at most one thumbnail per dish
        var makeThumb = isThumbnail || !dish.Images.Any(i => i.IsThumbnail);
        if (makeThumb)
            foreach (var i in dish.Images) i.IsThumbnail = false;

        dish.Images.Add(new DishImage_BIT240009 { ImageUrl = imageUrl, IsThumbnail = makeThumb });
        await _db.SaveChangesAsync();

        TempData["Success"] = "Msg.ImageAdded";
        return RedirectToAction(nameof(Detail), new { id = dishId });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SetThumbnail(int? imageId)
    {
        var img = await _db.DishImages.FirstOrDefaultAsync(i => i.Id == imageId);
        if (img == null) return NotFoundPage("Err.ImageNotFound");

        var all = await _db.DishImages.Where(i => i.DishId == img.DishId).ToListAsync();
        foreach (var i in all) i.IsThumbnail = i.Id == img.Id;
        await _db.SaveChangesAsync();

        TempData["Success"] = "Msg.ThumbSet";
        return RedirectToAction(nameof(Detail), new { id = img.DishId });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteImage(int? imageId)
    {
        var img = await _db.DishImages.FirstOrDefaultAsync(i => i.Id == imageId);
        if (img == null) return NotFoundPage("Err.ImageNotFound");

        var dishId = img.DishId;
        var wasThumb = img.IsThumbnail;
        _db.DishImages.Remove(img);
        await _db.SaveChangesAsync();

        if (wasThumb)
        {
            var next = await _db.DishImages.Where(i => i.DishId == dishId).OrderBy(i => i.Id).FirstOrDefaultAsync();
            if (next != null)
            {
                next.IsThumbnail = true;
                await _db.SaveChangesAsync();
            }
        }

        TempData["Success"] = "Msg.ImageDeleted";
        return RedirectToAction(nameof(Detail), new { id = dishId });
    }

    // ---------- shared validation: category exists + unique name inside a category ----------
    private async Task ValidateDish(Dish_BIT240009 d)
    {
        d.Name = d.Name?.Trim();

        var categoryOk = false;
        if (d.DishCategoryId > 0)
        {
            categoryOk = await _db.DishCategories.AnyAsync(c => c.Id == d.DishCategoryId);
            if (!categoryOk)
                ModelState.AddModelError(nameof(d.DishCategoryId), "Val.CategoryNotFound");
        }

        if (categoryOk && !string.IsNullOrEmpty(d.Name))
        {
            var name = d.Name;
            var duplicated = await _db.Dishes.AnyAsync(x =>
                x.DishCategoryId == d.DishCategoryId && x.Id != d.Id && x.Name == name);
            if (duplicated)
                ModelState.AddModelError(nameof(d.Name), "Val.NameDuplicate");
        }
    }
}
