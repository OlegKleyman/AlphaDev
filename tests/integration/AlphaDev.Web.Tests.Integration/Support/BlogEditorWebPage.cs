using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class BlogEditorWebPage : WebPage
    {
        public BlogEditorWebPage(IWebDriver driver, Uri baseUrl) : base(driver, baseUrl)
        {
        }

        public string Content
        {
            get
            {
                var prefix = Driver.FindElement(By.Name("prefix")).GetAttribute("value");
                return Driver.FindElement(By.Id($"{prefix}_Content")).Text;
            }
            set
            {
                var prefix = Driver.FindElement(By.Name("prefix")).GetAttribute("value");
                var content = Driver.FindElement(By.Id($"{prefix}_Content"));
                content.Clear();
                content.SendKeys(value);
            }
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

        public string Preview => Driver.FindElement(By.ClassName("md-preview")).GetAttribute("innerHTML").Trim();

        [NotNull]
        public IEnumerable<string> PageErrors
        {
            get { return Driver.FindElements(By.CssSelector("span[id$=\"-error\"]")).Select(element => element.Text); }
        }

        public void Submit()
        {
            Driver.FindElement(By.ClassName("btn-success")).Click();
        }

        public void TogglePreview()
        {
            Driver.FindElement(By.ClassName("btn-primary")).Click();
        }
    }
}