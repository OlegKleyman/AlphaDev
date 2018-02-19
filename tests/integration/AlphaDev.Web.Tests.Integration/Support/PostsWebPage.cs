using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using Optional;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class PostsWebPage : WebPage
    {
        public PostsWebPage(IWebDriver driver, Uri baseUrl) : base(driver, baseUrl)
        {
        }

        public IEnumerable<BlogPost> Posts
        {
            get
            {
                return Driver.FindElements(By.CssSelector(".blog .bubble")).Select(element =>
                    new BlogPost(element.FindElement(By.CssSelector(".title h3")).Text,
                        element.FindElement(By.CssSelector(".blog-content")).GetAttribute("innerHTML").Trim(),
                        new BlogDate(element.FindElement(By.CssSelector(".created-date")).Text,
                            (element.FindElements(By.CssSelector(".modified-date")).FirstOrDefault()?.Text)
                            .SomeNotNull()),
                        new NavigationLink(element.FindElement(By.CssSelector(".navigation-link a"))
                            .GetAttribute("href"))));
            }
        }

        public BlogPost Post { get; set; }
    }
}