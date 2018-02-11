using System;
using OpenQA.Selenium;

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
            .GetAttribute("innerHTML").Trim(), Driver.FindElement(
            By.CssSelector(
                "div.blog .dates")).Text);

        public override WebPage GoTo()
        {
            Driver.Navigate().GoToUrl(BaseUrl);

            return this;
        }
    }
}