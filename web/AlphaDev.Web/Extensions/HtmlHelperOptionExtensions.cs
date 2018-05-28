using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Optional;

namespace AlphaDev.Web.Extensions
{
    public static class HtmlHelperOptionExtensions
    {
        public static IHtmlContent DisplayFor<TModel, TResult>([NotNull] this IHtmlHelper<TModel> htmlHelper,
            Option<TResult> option)
        {
            return option.Map(result => htmlHelper.DisplayFor(model => result))
                .ValueOr(() => htmlHelper.Raw(string.Empty));
        }

        public static IHtmlContent Hidden<TModel, TResult>([NotNull] this IHtmlHelper<TModel> htmlHelper,
            string expression,
            Option<TResult> option, object htmlAttributes)
        {
            return option.Map(result => htmlHelper.Hidden(expression, result, htmlAttributes))
                .ValueOr(() => htmlHelper.Raw(string.Empty));
        }
    }
}