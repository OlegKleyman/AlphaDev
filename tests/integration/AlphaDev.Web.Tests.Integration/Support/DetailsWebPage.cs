using System;
using AlphaDev.Web.Tests.Integration.Extensions;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class DetailsWebPage : WebPage
    {
        public DetailsWebPage(IWebDriver driver, Uri baseUrl) : base(driver, baseUrl)
        {
        }

        public string Details => Driver.FindElement(By.CssSelector(".bubble div:nth-child(2)")).GetInnerHtml().Trim();
    }
}