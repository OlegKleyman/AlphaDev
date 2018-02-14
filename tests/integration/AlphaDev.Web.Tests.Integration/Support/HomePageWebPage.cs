using System;
using System.Linq;
using OpenQA.Selenium;
using Optional;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class HomePageWebPage : WebPage
    {
        public HomePageWebPage(IWebDriver driver, Uri baseUrl) : base(driver, baseUrl)
        {
        }

        public BlogPost LatestBlog => new BlogPost(Driver.FindElement(
            By.CssSelector(
                ".blog .title h3")).Text, Driver.FindElement(
                By.CssSelector(
                    ".blog .blog-content"))
            .GetAttribute("innerHTML").Trim(), new BlogDate(Driver.FindElement(
            By.CssSelector(
                ".blog .dates .created-date")).Text, Option.Some(Driver.FindElements(By.CssSelector(
            ".blog .dates .modified-date")).FirstOrDefault()?.Text).NotNull()));
    }
}