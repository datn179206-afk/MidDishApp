using Microsoft.AspNetCore.Mvc;

namespace MidDishApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => RedirectToAction("Index", "Dishes");

    public IActionResult Status(int code)
    {
        ViewBag.MessageKey = code == 404 ? "Err.PageNotFound" : "Err.Generic";
        ViewBag.Code = code;
        return View();
    }

    public IActionResult Error()
    {
        ViewBag.MessageKey = "Err.Generic";
        ViewBag.Code = 500;
        return View("Status");
    }
}
