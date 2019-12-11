using JetBrains.Annotations;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Extensions
{
    public static class WebElementExtensions
    {
        public static string GetInnerHtml([NotNull] this IWebElement element) => element.GetAttribute("innerHTML");
    }
}