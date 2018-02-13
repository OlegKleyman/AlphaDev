using System;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class PostsWebPage : WebPage
    {
        public PostsWebPage(IWebDriver driver, Uri baseUrl) : base(driver, baseUrl)
        {
        }
    }
}