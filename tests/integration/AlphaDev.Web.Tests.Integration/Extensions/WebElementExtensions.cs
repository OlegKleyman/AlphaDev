using JetBrains.Annotations;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Extensions
{
    public static class WebElementExtensions
    {
        public static string GetInnerHtml([NotNull] this IWebElement element)
        {
            return element.GetAttribute("innerHTML");
        }
    }
}
