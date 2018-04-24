using System;
using System.Collections.Generic;
using System.Linq;
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
            get => Driver.FindElement(By.Id("Content")).Text;
            set
            {
                var content = Driver.FindElement(By.Id("Content"));
                content.Clear();
                content.SendKeys(value);
            }
        }

        public string BlogTitle
        {
            get => Driver.FindElement(By.Id("Title")).Text;
            set
            {
                var title = Driver.FindElement(By.Id("Title"));
                title.Clear();
                title.SendKeys(value);
            }
        }

        public string Preview => Driver.FindElement(By.ClassName("md-preview")).GetAttribute("innerHTML").Trim();

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