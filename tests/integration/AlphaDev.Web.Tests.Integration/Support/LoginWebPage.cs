using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration.Support
{
    public class LoginWebPage : WebPage
    {
        public LoginWebPage(IWebDriver driver, Uri baseUrl) : base(driver, baseUrl)
        {
        }

        public string Username
        {
            get => Driver.FindElement(By.Id("Username")).Text;
            set => Driver.FindElement(By.Id("Username")).SendKeys(value);
        }

        public string Password
        {
            get => Driver.FindElement(By.Id("Password")).Text;
            set => Driver.FindElement(By.Id("Password")).SendKeys(value);
        }

        [NotNull]
        public IEnumerable<string> ValidationSummary =>
            Driver.FindElements(By.CssSelector("form div ul li")).Select(element => element.Text);

        public void Submit()
        {
            Driver.FindElement(By.CssSelector("form div button")).Click();
        }
    }
}