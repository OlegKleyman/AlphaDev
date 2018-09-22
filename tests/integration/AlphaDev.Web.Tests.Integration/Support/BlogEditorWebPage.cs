using System;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class BlogEditorWebPage : EditorWebPageBase
    {
        public BlogEditorWebPage(IWebDriver driver, Uri baseUrl) : base(driver, baseUrl, "Content")
        {
        }

        public string BlogTitle
        {
            get
            {
                var prefix = Driver.FindElement(By.Name("prefix")).GetAttribute("value");
                return Driver.FindElement(By.Id($"{prefix}_Title")).Text;
            }
            set
            {
                var prefix = Driver.FindElement(By.Name("prefix")).GetAttribute("value");
                var title = Driver.FindElement(By.Id($"{prefix}_Title"));
                title.Clear();
                title.SendKeys(value);
            }
        }
    }
}