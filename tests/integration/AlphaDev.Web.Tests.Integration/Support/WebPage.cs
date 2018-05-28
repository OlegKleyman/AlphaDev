using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public abstract class WebPage
    {
        protected readonly IWebDriver Driver;

        protected WebPage(IWebDriver driver, Uri baseUrl)
        {
            BaseUrl = baseUrl;
            Driver = driver;
        }

        public Uri BaseUrl { get; }

        public string Title => Driver.Title;

        [NotNull]
        public IEnumerable<NavigationElement> Navigation => Driver
            .FindElements(By.CssSelector("div.navbar-collapse a, div.navbar-collapse button"))
            .Select(NavigationElement.FromWebElement);

        [NotNull]
        public virtual WebPage GoTo()
        {
            Driver.Navigate().GoToUrl(BaseUrl);

            return this;
        }

        public void GoTo(int id)
        {
            Driver.Navigate().GoToUrl($"{BaseUrl.AbsoluteUri.Trim('/')}/{id}");
        }
    }
}