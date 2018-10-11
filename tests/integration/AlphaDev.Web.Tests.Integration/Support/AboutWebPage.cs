using System;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class AboutWebPage : DetailsWebPage
    {
        public AboutWebPage(IWebDriver driver, Uri baseUrl) : base(driver, new Uri(baseUrl, "info/about/"))
        {
        }

        public override void EditValue()
        {
            Driver.FindElement(By.CssSelector("a[href*=\"/info/about/edit\"]")).Click();
        }
    }
}