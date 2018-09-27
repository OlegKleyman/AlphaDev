using System;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class AboutEditorWebPage : EditorWebPageBase
    {
        public AboutEditorWebPage(IWebDriver driver, Uri baseUrl) : base(driver, new Uri(baseUrl, "edit"), "Value")
        {
        }
    }
}