using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MidDishApp.Data;
using MidDishApp.Models;

namespace MidDishApp.Controllers;

public class CategoriesController : BaseController
{
    private readonly AppDbContext _db;
    public CategoriesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var list = await _db.DishCategories.AsNoTracking()
            .Include(c => c.Dishes)
            .OrderBy(c => c.Id)
            .ToListAsync();
        return View(list);
    }

    public IActionResult Create() => View(new DishCategory_BIT240009());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description")] DishCategory_BIT240009 cat)
    {
        await ValidateCategory(cat);
        if (!ModelState.IsValid) return View(cat);

        _db.DishCategories.Add(cat);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Msg.Saved";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFoundPage("Err.CategoryNotFound");
        var cat = await _db.DishCategories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        if (cat == null) return NotFoundPage("Err.CategoryNotFound");
        return View(cat);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Description")] DishCategory_BIT240009 input)
    {
        if (id == null || id != input.Id) return NotFoundPage("Err.CategoryNotFound");
        var cat = await _db.DishCategories.FirstOrDefaultAsync(c => c.Id == id);
        if (cat == null) return NotFoundPage("Err.CategoryNotFound");

        await ValidateCategory(input);
        if (!ModelState.IsValid) return View(input);

        cat.Name = input.Name;
        cat.Description = input.Description;
        await _db.SaveChangesAsync();
        TempData["Success"] = "Msg.Saved";
        return RedirectToAction(nameof(Index));
    }

    // A category that still has dishes cannot be deleted.
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFoundPage("Err.CategoryNotFound");
        var cat = await _db.DishCategories.FirstOrDefaultAsync(c => c.Id == id);
        if (cat == null) return NotFoundPage("Err.CategoryNotFound");

        if (await _db.Dishes.AnyAsync(d => d.DishCategoryId == id))
        {
            TempData["Error"] = "Msg.CategoryInUse";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            _db.DishCategories.Remove(cat);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Msg.Deleted";
        }
        catch (DbUpdateException)
        {
            TempData["Error"] = "Msg.DeleteFailed";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidateCategory(DishCategory_BIT240009 c)
    {
        c.Name = c.Name?.Trim();
        if (string.IsNullOrEmpty(c.Name)) return;

        var name = c.Name;
        if (await _db.DishCategories.AnyAsync(x => x.Id != c.Id && x.Name == name))
            ModelState.AddModelError(nameof(c.Name), "Val.CatNameDuplicate");
    }
}
