using System;
using AlphaDev.Web.Tests.Integration.Extensions;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class AboutWebPage : WebPage
    {
        public AboutWebPage(IWebDriver driver, Uri baseUrl) : base(driver, new Uri(baseUrl, "info/about/"))
        {
            Edit = new AboutEditorWebPage(driver, BaseUrl);
            Create = new AboutCreateEditorWebPage(driver, BaseUrl);
        }

        public string Details => Driver.FindElement(By.CssSelector(".bubble div:nth-child(2)")).GetInnerHtml().Trim();

        public AboutEditorWebPage Edit { get; }
        public AboutCreateEditorWebPage Create { get; }

        public void EditAbout()
        {
            Driver.FindElement(By.CssSelector("a[href*=\"/info/about/edit\"]")).Click();
        }
    }
}