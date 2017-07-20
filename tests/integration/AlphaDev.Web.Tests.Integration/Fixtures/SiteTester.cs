using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class SiteTester : IDisposable
    {
        public SiteTester()
        {
            var path = Environment.GetEnvironmentVariable("PATH");

            Environment.SetEnvironmentVariable("PATH", path + ";.");

            Driver = new FirefoxDriver();
        }

        public IWebDriver Driver { get; set; }

        public void Dispose()
        {
            Driver.Dispose();
        }
    }
}