using System;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class LoginWebPage:WebPage
    {
        public LoginWebPage(IWebDriver driver, Uri baseUrl) : base(driver, baseUrl)
        {
        }
    }
}