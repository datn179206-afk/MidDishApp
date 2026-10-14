using System.Net;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using MidDishApp.Services;

namespace MidDishApp.Helpers;

public static class DishDisplay
{
    /// <summary>Computed at display time, no database column.</summary>
    public static string TimeKey(int minutes) => minutes <= 15 ? "Time.Quick" : "Time.Regular";
}

public static class HtmlExtensions
{
    /// <summary>Renders the (translated) validation errors of one field.</summary>
    public static IHtmlContent Err(this IHtmlHelper html, string field)
    {
        if (!html.ViewData.ModelState.TryGetValue(field, out var entry) || entry.Errors.Count == 0)
            return HtmlString.Empty;

        var loc = (ILocalizer)html.ViewContext.HttpContext.RequestServices.GetService(typeof(ILocalizer));
        var sb = new StringBuilder();
        foreach (var e in entry.Errors)
        {
            var key = string.IsNullOrEmpty(e.ErrorMessage) ? "Val.Invalid" : e.ErrorMessage;
            sb.Append("<div class=\"text-danger small\">")
              .Append(WebUtility.HtmlEncode(loc[key]))
              .Append("</div>");
        }
        return new HtmlString(sb.ToString());
    }
}
