namespace AlphaDev.Web.Tests.Integration.Features
{
    using System;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;

    using AlphaDev.Core.Data.Sql.Contexts;

    using FluentAssertions;

    using LightBDD.XUnit2;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    using Omego.Extensions.QueryableExtensions;

    using OpenQA.Selenium;

    using Xunit;
    using Xunit.Abstractions;

    public partial class Homepage_feature : FeatureFixture, IDisposable, IClassFixture<SiteTester>
    {
        private readonly IWebDriver driver;

        public Homepage_feature(ITestOutputHelper output, SiteTester siteTester)
            : base(output)
        {
            driver = siteTester.Value;
            
            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("connectionstrings.json", false, true).Build();
        }
        
        public void Dispose() {}

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
            var blogContext = new BlogContext(
                string.Format(
                    CultureInfo.InvariantCulture,
                    Configuration.GetConnectionString("integration"),
                    Directory.GetCurrentDirectory()));
            
            blogContext.Database.Migrate();

            blogContext.Blogs.OrderBy(blog => blog.Created)
                .SingleOrThrow(
                    new InvalidOperationException("No blogs found."),
                    new InvalidOperationException("Multiple blogs found")).ShouldBeEquivalentTo(
                    new { Created = driver.FindElement(By.CssSelector("div.blog .title")) },
                    options => options.Including(blog => blog.Created).Including(blog => blog.Content)
                        .Including(blog => blog.Title));
        }

        public readonly IConfigurationRoot Configuration;
    }
}