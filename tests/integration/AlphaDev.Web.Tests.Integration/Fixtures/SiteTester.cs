using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.PhantomJS;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class SiteTester : IDisposable
    {
        public SiteTester()
        {
            const string pathEnvironmentVariableName = "PATH";
            var path = Environment.GetEnvironmentVariable(pathEnvironmentVariableName);

            Environment.SetEnvironmentVariable(pathEnvironmentVariableName, path + ";.");

            Driver = new PhantomJSDriver();
            Driver.Manage().Window.Maximize();
        }

        public IWebDriver Driver { get; set; }

        public void Dispose()
        {
            Driver.Dispose();
        }
    }
}