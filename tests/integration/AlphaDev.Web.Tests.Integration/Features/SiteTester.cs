namespace AlphaDev.Web.Tests.Integration.Features
{
    using System;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Firefox;

    public class SiteTester : IDisposable
    {
        public SiteTester()
        {
            var path = Environment.GetEnvironmentVariable("PATH");

            Environment.SetEnvironmentVariable("PATH", path + ";.");

            Value = new FirefoxDriver();
        }
        public IWebDriver Value { get; set; }

        public void Dispose() => Value.Dispose();
    }
}