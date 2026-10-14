using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidDishApp.Models;

[Table("BIT240009_Dishes")]
public class Dish_BIT240009
{
    [Column("BIT240009_Id")]
    public int Id { get; set; }

    [Column("BIT240009_Name")]
    [Required(ErrorMessage = "Val.NameRequired")]
    [StringLength(100, ErrorMessage = "Val.NameLength")]
    public string Name { get; set; }

    [Column("BIT240009_Price", TypeName = "decimal(18,2)")]
    [Range(typeof(decimal), "0.01", "999999999", ErrorMessage = "Val.PriceRange")]
    public decimal Price { get; set; }

    [Column("BIT240009_PreparationTime")]
    [Range(1, int.MaxValue, ErrorMessage = "Val.TimeRange")]
    public int PreparationTime { get; set; }

    [Column("BIT240009_IsAvailable")]
    public bool IsAvailable { get; set; }

    [Column("BIT240009_Description")]
    [StringLength(500, ErrorMessage = "Val.DescLength")]
    public string Description { get; set; }

    [Column("BIT240009_DishCategoryId")]
    [Range(1, int.MaxValue, ErrorMessage = "Val.CategoryRequired")]
    public int DishCategoryId { get; set; }

    public DishCategory_BIT240009 DishCategory { get; set; }

    public ICollection<DishImage_BIT240009> Images { get; set; } = new List<DishImage_BIT240009>();
}
