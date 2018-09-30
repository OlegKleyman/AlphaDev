using System;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class ContactWebPage : DetailsWebPage
    {
        public ContactWebPage(IWebDriver driver, Uri baseUrl) : base(driver, new Uri(baseUrl, "info/contact/"))
        {
        }
    }
}