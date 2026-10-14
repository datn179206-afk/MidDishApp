using Microsoft.AspNetCore.Mvc;

namespace MidDishApp.Controllers;

public abstract class BaseController : Controller
{
    /// <summary>404 page with a translated message (Dish / DishCategory / DishImage not found...).</summary>
    protected IActionResult NotFoundPage(string messageKey)
    {
        Response.StatusCode = 404;
        ViewBag.MessageKey = messageKey;
        return View("NotFoundItem");
    }
}
