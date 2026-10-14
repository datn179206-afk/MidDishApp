using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidDishApp.Models;

[Table("BIT240009_DishCategories")]
public class DishCategory_BIT240009
{
    [Column("BIT240009_Id")]
    public int Id { get; set; }

    [Column("BIT240009_Name")]
    [Required(ErrorMessage = "Val.CatNameRequired")]
    [StringLength(100, ErrorMessage = "Val.NameLength")]
    public string Name { get; set; }

    [Column("BIT240009_Description")]
    [StringLength(500, ErrorMessage = "Val.DescLength")]
    public string Description { get; set; }

    public ICollection<Dish_BIT240009> Dishes { get; set; } = new List<Dish_BIT240009>();
}
