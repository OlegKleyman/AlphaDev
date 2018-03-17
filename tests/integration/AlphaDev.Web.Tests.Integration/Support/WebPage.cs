using System;
using System.Collections.Generic;
using System.Globalization;
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
                var s = Driver.PageSource;
                var readOnlyCollection = Driver.FindElements(By.CssSelector("ul.navbar-nav a"));
                return readOnlyCollection
                    .Select(element => new AnchorElement(element.Text, element.GetAttribute("href")));
            }
        }

        public virtual WebPage GoTo()
        {
            Driver.Navigate().GoToUrl(BaseUrl);

            return this;
        }

        public void GoTo(int id)
        {
            Driver.Navigate().GoToUrl(new Uri(BaseUrl, id.ToString(CultureInfo.InvariantCulture)));
        }
    }
}