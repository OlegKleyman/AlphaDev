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
                "div.blog .title h2")).Text, Driver.FindElement(
                By.CssSelector(
                    "div.blog .content"))
            .GetAttribute("innerHTML").Trim(), new BlogDate(Driver.FindElement(
            By.CssSelector(
                "div.blog .dates .created-date")).Text, Option.Some(Driver.FindElements(By.CssSelector(
            "div.blog .dates .modified-date")).FirstOrDefault()?.Text).NotNull()));
    }

    public class BlogDate
    {
        public BlogDate(string created, Option<string> modified)
        {
            Created = created;
            Modified = modified;
        }

        public string Created { get; }
        public Option<string> Modified { get; }
    }
}