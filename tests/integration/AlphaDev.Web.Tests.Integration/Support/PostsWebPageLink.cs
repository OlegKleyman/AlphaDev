using System;
using JetBrains.Annotations;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class PostsWebPageLink : INavigable<Page>
    {
        private readonly IWebDriver _driver;

        public PostsWebPageLink(IWebDriver driver, Page page)
        {
            Page = page;
            _driver = driver;
        }

        public Page Page { get; }

        [NotNull]
        public Page GoTo()
        {
            _driver.Navigate()
                .GoToUrl(Page.Attributes.Url.ValueOr(() =>
                    throw new InvalidOperationException("This page is not a link.")));
            return new Page(Page.Identity, Page.DisplayFormat, Page.Attributes.ToActive(Page.Identity.Number));
        }
    }
}