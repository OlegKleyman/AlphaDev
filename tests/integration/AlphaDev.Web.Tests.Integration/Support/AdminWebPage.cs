using System;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class AdminWebPage : WebPage
    {
        public AdminWebPage(IWebDriver driver, Uri baseUrl) : base(driver, baseUrl)
        {
        }
    }
}