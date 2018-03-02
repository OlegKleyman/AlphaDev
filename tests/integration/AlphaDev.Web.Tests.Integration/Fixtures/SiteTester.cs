using System;
using AlphaDev.Web.Tests.Integration.Support;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class SiteTester : IDisposable
    {
        public SiteTester(Uri baseUrl)
        {
            const string pathEnvironmentVariableName = "PATH";
            var path = Environment.GetEnvironmentVariable(pathEnvironmentVariableName);

            Environment.SetEnvironmentVariable(pathEnvironmentVariableName, path + ";.");

            Driver = new PhantomJSDriver();
            Driver.Manage().Window.Maximize();

            HomePage = new HomePageWebPage(Driver, baseUrl);
            Posts = new PostsWebPage(Driver, new Uri(baseUrl, "posts/"));
            Error = new ErrorWebPage(Driver, new Uri(baseUrl, "error/"));
            Admin = new AdminWebPage(Driver, new Uri(baseUrl, "admin/"));
            Login = new LoginWebPage(Driver, new Uri(baseUrl, "login/"));
        }

        public IWebDriver Driver { get; }

        public HomePageWebPage HomePage { get; }
        public PostsWebPage Posts { get; }
        public ErrorWebPage Error { get; }
        public AdminWebPage Admin { get; }
        public LoginWebPage Login { get; }

        public void Dispose()
        {
            Driver.Dispose();
        }
    }
}