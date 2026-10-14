using Microsoft.AspNetCore.Mvc;

namespace MidDishApp.Controllers;

public class LanguageController : Controller
{
    // GET /Language/Set?lang=vi&returnUrl=/Dishes?Q=pho  -> keeps the current page + filters
    public IActionResult Set(string lang, string returnUrl)
    {
        lang = lang == "vi" ? "vi" : "en";

        Response.Cookies.Append("lang", lang, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            IsEssential = true,
            Path = "/"
        });

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return LocalRedirect(returnUrl);

        return RedirectToAction("Index", "Dishes");
    }
}
