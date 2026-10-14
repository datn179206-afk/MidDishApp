namespace MidDishApp.Models;

public class DishFilter
{
    public string Q { get; set; }
    public int? CategoryId { get; set; }
    /// <summary>"", "available" or "unavailable"</summary>
    public string Status { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    /// <summary>"", "price_asc", "price_desc", "time_asc"</summary>
    public string Sort { get; set; }
}

public class DishIndexVm
{
    public DishFilter Filter { get; set; } = new DishFilter();
    public List<DishCategory_BIT240009> Categories { get; set; } = new();
    public List<Dish_BIT240009> Dishes { get; set; } = new();
    /// <summary>Translation key of a filter error, if any.</summary>
    public string RangeError { get; set; }
}
