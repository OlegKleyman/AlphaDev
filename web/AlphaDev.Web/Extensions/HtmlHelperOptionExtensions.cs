using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Optional;

namespace AlphaDev.Web.Extensions
{
    public static class HtmlHelperOptionExtensions
    {
        public static IHtmlContent DisplayFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Option<TResult> n)
        {
            return n.Map(result => htmlHelper.DisplayFor(model => result))
                .ValueOr(() => htmlHelper.Raw(string.Empty));
        }
    }
}