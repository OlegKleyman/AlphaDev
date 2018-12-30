using System;
using JetBrains.Annotations;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class PostsWebPageLink : INavigable<PostsWebPage>
    {
        public Page Page { get; }
        private readonly IWebDriver _driver;

        public PostsWebPageLink(IWebDriver driver, [NotNull] Uri baseUri, Page page)
        {
            Page = page;
            _driver = driver;
            Url = new Uri(baseUri, $"/page/{page.Identity.Number}");
        }

        public Uri Url { get; }

        [NotNull]
        public PostsWebPage GoTo()
        {
            _driver.Navigate().GoToUrl(Url);
            return  new PostsWebPage(_driver, Url, Page);
        }
    }
}