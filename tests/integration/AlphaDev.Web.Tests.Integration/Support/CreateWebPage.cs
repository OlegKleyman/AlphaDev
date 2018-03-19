using System;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class CreateWebPage:WebPage
    {
        public CreateWebPage(IWebDriver driver, Uri baseUrl) : base(driver, baseUrl)
        {
        }

        public string Content
        {
            get => Driver.FindElement(By.Id("Content")).Text;
            set => Driver.FindElement(By.Id("Content")).SendKeys(value);
        }

        public string BlogTitle
        {
            get => Driver.FindElement(By.Id("Title")).Text;
            set => Driver.FindElement(By.Id("Title")).SendKeys(value);
        }

        public void Submit()
        {
            Driver.FindElement(By.ClassName("btn-success")).Click();
        }
    }
}