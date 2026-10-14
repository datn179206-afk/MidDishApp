using MidDishApp.Models;

namespace MidDishApp.Data;

public static class SeedData
{
    public static void Init(AppDbContext db)
    {
        if (db.DishCategories.Any()) return;

        var main = new DishCategory_BIT240009 { Name = "Món chính", Description = "Các món ăn chính" };
        var dessert = new DishCategory_BIT240009 { Name = "Tráng miệng", Description = "Các món ngọt, tráng miệng" };
        var drink = new DishCategory_BIT240009 { Name = "Đồ uống", Description = "Cà phê, trà, nước ép" };
        db.DishCategories.AddRange(main, dessert, drink);

        var pho = new Dish_BIT240009
        {
            Name = "Phở bò", Price = 55000, PreparationTime = 20, IsAvailable = true,
            Description = "Phở bò truyền thống", DishCategory = main
        };
        pho.Images.Add(new DishImage_BIT240009 { ImageUrl = "https://picsum.photos/seed/pho1/400/300", IsThumbnail = true });
        pho.Images.Add(new DishImage_BIT240009 { ImageUrl = "https://picsum.photos/seed/pho2/400/300", IsThumbnail = false });

        var buncha = new Dish_BIT240009
        {
            Name = "Bún chả", Price = 50000, PreparationTime = 15, IsAvailable = true,
            Description = "Bún chả Hà Nội", DishCategory = main
        };
        buncha.Images.Add(new DishImage_BIT240009 { ImageUrl = "https://picsum.photos/seed/buncha/400/300", IsThumbnail = true });

        var che = new Dish_BIT240009
        {
            Name = "Chè ba màu", Price = 25000, PreparationTime = 10, IsAvailable = true,
            Description = "Chè ba màu mát lạnh", DishCategory = dessert
        };
        var flan = new Dish_BIT240009
        {
            Name = "Bánh flan", Price = 20000, PreparationTime = 8, IsAvailable = false,
            Description = "Bánh flan caramel (tạm ngừng bán)", DishCategory = dessert
        };
        var coffee = new Dish_BIT240009
        {
            Name = "Cà phê sữa đá", Price = 22000, PreparationTime = 5, IsAvailable = true,
            Description = "Cà phê phin sữa đá", DishCategory = drink
        };

        db.Dishes.AddRange(pho, buncha, che, flan, coffee);
        db.SaveChanges();
    }
}
