using System;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class SingleValueCreateEditorWebPage : EditorWebPageBase
    {
        public SingleValueCreateEditorWebPage(IWebDriver driver, Uri baseUrl) : base(driver, new Uri(baseUrl, "create"),
            "Value")
        {
        }
    }
}