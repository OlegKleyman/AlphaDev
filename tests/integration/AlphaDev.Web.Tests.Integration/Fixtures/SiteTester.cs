using System;
using AlphaDev.Web.Tests.Integration.Support;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class SiteTester : IDisposable
    {
        public SiteTester(Uri baseUrl)
        {
            const string pathEnvironmentVariableName = "PATH";
            var path = Environment.GetEnvironmentVariable(pathEnvironmentVariableName);

            Environment.SetEnvironmentVariable(pathEnvironmentVariableName, path + ";.");

            var options = new ChromeOptions();
            options.AddArguments("headless");

            Driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(".", "chromedriver.exe"), options);

            Driver.Manage().Window.Maximize();

            HomePage = new HomePageWebPage(Driver, baseUrl);
            Posts = new PostsWebPage(Driver, new Uri(baseUrl, "posts/"));
            Error = new ErrorWebPage(Driver, new Uri(baseUrl, "error/"));
            Admin = new AdminWebPage(Driver, new Uri(baseUrl, "admin/"));
            Login = new LoginWebPage(Driver, new Uri(baseUrl, "account/login/"));
            About = new AboutWebPage(Driver, baseUrl);
        }

        public IWebDriver Driver { get; }

        public HomePageWebPage HomePage { get; }
        public PostsWebPage Posts { get; }
        public ErrorWebPage Error { get; }
        public AdminWebPage Admin { get; }
        public LoginWebPage Login { get; }
        public AboutWebPage About { get; }

        public void Dispose()
        {
            Driver.Dispose();
        }
    }
}