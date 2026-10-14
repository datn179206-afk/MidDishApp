using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidDishApp.Models;

[Table("BIT240009_DishImages")]
public class DishImage_BIT240009
{
    [Column("BIT240009_Id")]
    public int Id { get; set; }

    [Column("BIT240009_ImageUrl")]
    [Required(ErrorMessage = "Val.UrlRequired")]
    [StringLength(500, ErrorMessage = "Val.UrlInvalid")]
    public string ImageUrl { get; set; }

    [Column("BIT240009_IsThumbnail")]
    public bool IsThumbnail { get; set; }

    [Column("BIT240009_DishId")]
    public int DishId { get; set; }

    public Dish_BIT240009 Dish { get; set; }
}
