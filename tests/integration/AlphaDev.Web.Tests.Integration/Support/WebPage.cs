using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<AnchorElement> Navigation
        {
            get
            {
                return Driver.FindElements(By.CssSelector("ul.navbar-nav a"))
                    .Select(element => new AnchorElement(element.Text, element.GetAttribute("href")));
            }
        }

        public abstract WebPage GoTo();
    }
}