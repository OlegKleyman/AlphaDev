using System;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class ErrorWebPage : WebPage
    {
        public ErrorWebPage(IWebDriver driver, Uri baseUrl) : base(driver, baseUrl)
        {
        }

        public string Message => Driver.FindElement(By.TagName("h2")).Text;
        public string Status => Driver.FindElement(By.TagName("h1")).Text;
    }
}