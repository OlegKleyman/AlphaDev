using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Optional;

namespace AlphaDev.Web.Core.Extensions
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

        public static IHtmlContent ActionLink<TModel, TResult>([NotNull] this IHtmlHelper<TModel> htmlHelper,
            Option<TResult> option, string linkText, string actionName, string controllerName,
            Func<TResult, object> routeValues, object htmlAttributes)
        {
            return option.Map(result => htmlHelper.ActionLink(linkText, actionName, controllerName, null, null, null,
                             routeValues(result), htmlAttributes))
                         .ValueOr(() => htmlHelper.Raw(string.Empty));
        }
    }
}