using System;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class SingleValueEditEditorWebPage : EditorWebPageBase
    {
        public SingleValueEditEditorWebPage(IWebDriver driver, Uri baseUrl) : base(driver, new Uri(baseUrl, "edit"),
            "Value")
        {
        }
    }
}