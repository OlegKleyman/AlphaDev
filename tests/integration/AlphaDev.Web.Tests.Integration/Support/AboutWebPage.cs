using System;
using AlphaDev.Web.Tests.Integration.Extensions;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class AboutWebPage : DetailsWebPage
    {
        public AboutWebPage(IWebDriver driver, Uri baseUrl) : base(driver, new Uri(baseUrl, "info/about/"))
        {
            Edit = new AboutEditorWebPage(driver, BaseUrl);
        }

        public AboutEditorWebPage Edit { get; }

        public void EditAbout()
        {
            Driver.FindElement(By.CssSelector("a[href*=\"/info/about/edit\"]")).Click();
        }
    }
}