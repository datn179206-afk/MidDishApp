using Microsoft.EntityFrameworkCore;
using MidDishApp.Models;

namespace MidDishApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<DishCategory_BIT240009> DishCategories => Set<DishCategory_BIT240009>();
    public DbSet<Dish_BIT240009> Dishes => Set<Dish_BIT240009>();
    public DbSet<DishImage_BIT240009> DishImages => Set<DishImage_BIT240009>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<DishCategory_BIT240009>()
            .HasIndex(c => c.Name).IsUnique();

        b.Entity<Dish_BIT240009>(e =>
        {
            // 1 category -> many dishes; cannot delete a category that still has dishes
            e.HasOne(d => d.DishCategory)
             .WithMany(c => c.Dishes)
             .HasForeignKey(d => d.DishCategoryId)
             .OnDelete(DeleteBehavior.Restrict);

            // dish name unique inside one category
            e.HasIndex(d => new { d.DishCategoryId, d.Name }).IsUnique();
        });

        // 1 dish -> many images
        b.Entity<DishImage_BIT240009>()
            .HasOne(i => i.Dish)
            .WithMany(d => d.Images)
            .HasForeignKey(i => i.DishId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
