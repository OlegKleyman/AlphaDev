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

    public partial class Homepage_feature : FeatureFixture, IClassFixture<SiteTester>, IClassFixture<DatabaseFixture>
    {
        private readonly SiteTester siteTester;

        private readonly DatabaseFixture databaseFixture;

        public Homepage_feature(ITestOutputHelper output, SiteTester siteTester, DatabaseFixture databaseFixture)
            : base(output)
        {
            this.siteTester = siteTester;
            this.databaseFixture = databaseFixture;
        }

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

                siteTester.Driver.Navigate().GoToUrl(url);
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

        private void Then_it_should_load() => siteTester.Driver.Title.ShouldBeEquivalentTo("Home - AlphaDev");

        private void Then_it_should_display_navigation_links() => siteTester.Driver.FindElements(By.CssSelector("ul.navbar-nav a"))
            .Select(element => element.Text).ShouldBeEquivalentTo(new[] { "Posts", "About", "Contact" });

        private void Then_it_should_display_the_latest_blog_post()
        {
            databaseFixture.BlogContext.Database.Migrate();

            databaseFixture.BlogContext.Blogs.OrderBy(blog => blog.Created)
                .SingleOrThrow(
                    new InvalidOperationException("No blogs found."),
                    new InvalidOperationException("Multiple blogs found")).ShouldBeEquivalentTo(
                    new { Created = siteTester.Driver.FindElement(By.CssSelector("div.blog .title")) },
                    options => options.Including(blog => blog.Created).Including(blog => blog.Content)
                        .Including(blog => blog.Title));
        }

    }
}