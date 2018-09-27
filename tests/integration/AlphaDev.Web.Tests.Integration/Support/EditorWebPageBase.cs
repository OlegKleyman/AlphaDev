using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class EditorWebPageBase : WebPage
    {
        private readonly string _editorElement;

        public EditorWebPageBase(IWebDriver driver, Uri baseUrl, [CanBeNull] string editorElement) : base(driver,
            baseUrl)
        {
            if (!string.IsNullOrWhiteSpace(editorElement)) _editorElement = $"_{editorElement}";
        }

        public string Content
        {
            get
            {
                var prefix = Driver.FindElement(By.Name("prefix")).GetAttribute("value");
                return Driver.FindElement(By.Id($"{prefix}{_editorElement}")).Text;
            }
            set
            {
                var prefix = Driver.FindElement(By.Name("prefix")).GetAttribute("value");
                var content = Driver.FindElement(By.Id($"{prefix}{_editorElement}"));
                content.Clear();
                content.SendKeys(value);
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