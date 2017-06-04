namespace AlphaDev.Web.Tests.Integration.Features
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;

    using FluentAssertions;

    using LightBDD.XUnit2;

    using Microsoft.AspNetCore.Hosting;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;

    using Xunit.Abstractions;

    public partial class Homepage_feature : FeatureFixture, IDisposable
    {
        private readonly IWebDriver driver;

        public Homepage_feature(ITestOutputHelper output)
            : base(output)
        {
            var path = Environment.GetEnvironmentVariable("PATH");

            Environment.SetEnvironmentVariable("PATH", path + ";.");

            driver = new FirefoxDriver();
        }

        public void Dispose() => driver.Dispose();

        private void Given_i_am_a_user()
        {
        }

        private void When_i_go_to_the_homepage()
        {
            var url = $"http://127.0.0.1:{GetOpenPort()}";
            using (var host = new WebHostBuilder()
                .UseContentRoot(Path.GetFullPath(@"..\..\..\..\..\..\web\AlphaDev.Web")).UseKestrel()
                .UseStartup<Startup>().UseUrls(url).Build())
            {
                host.Start();

                driver.Navigate().GoToUrl(url);
            }
        }

        private int GetOpenPort()
        {
            using (var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                const int randomOpenPort = 0;

                sock.Bind(new IPEndPoint(IPAddress.Loopback, randomOpenPort));

                return ((IPEndPoint)sock.LocalEndPoint).Port;
            }
        }

        private void Then_it_should_load() => driver.Title.ShouldBeEquivalentTo("Home - AlphaDev");

        private void Then_it_should_display_navigation_links() => driver.FindElements(By.CssSelector("ul.navbar-nav a"))
            .Select(element => element.Text).ShouldBeEquivalentTo(new[] { "Posts", "About", "Contact" });

        private void Then_it_should_display_the_latest_blog_post()
        {
            throw new NotImplementedException();
        }
    }
}