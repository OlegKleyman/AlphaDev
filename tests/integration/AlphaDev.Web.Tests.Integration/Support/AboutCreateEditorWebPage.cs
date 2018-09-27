using System;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class AboutCreateEditorWebPage : EditorWebPageBase
    {
        public AboutCreateEditorWebPage(IWebDriver driver, Uri baseUrl) : base(driver, new Uri(baseUrl, "create"),
            "Value")
        {
        }
    }
}