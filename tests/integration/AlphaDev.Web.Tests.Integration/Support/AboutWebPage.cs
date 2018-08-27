using System;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class AboutWebPage : WebPage
    {
        public AboutWebPage(IWebDriver driver, Uri baseUrl) : base(driver, new Uri(baseUrl, "info/about/"))
        {
        }

        public string Details
        {
            get { return Driver.FindElement(By.CssSelector(".bubble div:nth-child(2)")).Text; }
        }
    }
}