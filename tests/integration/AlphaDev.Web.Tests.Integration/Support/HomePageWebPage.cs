using System;
using System.Linq;
using OpenQA.Selenium;
using Optional;
using Optional.Collections;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class HomePageWebPage : WebPage
    {
        public HomePageWebPage(IWebDriver driver, Uri baseUrl) : base(driver, baseUrl)
        {
        }

        public Option<BlogPost> LatestBlog
        {
            get
            {
                var blog = Driver.FindElements(By.ClassName("blog")).FirstOrNone().Map(element =>
                    new BlogPost(element.FindElement(
                        By.CssSelector(
                            ".title h3")).Text, element.FindElement(
                            By.CssSelector(
                                ".bubble-content"))
                        .GetAttribute("innerHTML").Trim(), new BlogDate(element.FindElement(
                        By.CssSelector(
                            ".dates .created-date")).Text, Option.Some(element.FindElements(By.CssSelector(
                        ".dates .modified-date")).FirstOrDefault()?.Text).NotNull())));

                return blog;
            }
        }

        public void DeleteBlog()
        {
            Driver.FindElement(By.CssSelector(".blog button.delete")).Click();
        }

        public void EditBlog()
        {
            Driver.FindElement(By.CssSelector("a[href*=\"/posts/edit/\"]")).Click();
        }
    }
}