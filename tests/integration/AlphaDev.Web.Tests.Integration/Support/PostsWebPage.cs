using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using OpenQA.Selenium;
using Optional;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class PostsWebPage : WebPage
    {
        public PostsWebPage(IWebDriver driver, Uri baseUrl) : base(driver, baseUrl)
        {
            Create = new BlogEditorWebPage(Driver, new Uri(BaseUrl, "create"));
            Edit = new BlogEditorWebPage(Driver, new Uri(BaseUrl, "edit"));
        }

        [NotNull]
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

        [NotNull]
        public BlogPost Post => new BlogPost(Driver.FindElement(By.CssSelector(".blog .title h3")).Text,
            Driver.FindElement(By.CssSelector(".blog .blog-content")).GetAttribute("innerHTML").Trim(),
            new BlogDate(Driver.FindElement(By.CssSelector(".blog .created-date")).Text,
                (Driver.FindElements(By.CssSelector(".blog .modified-date")).FirstOrDefault()?.Text)
                .SomeNotNull()));

        public BlogEditorWebPage Create { get; }

        public BlogEditorWebPage Edit { get; }
    }
}