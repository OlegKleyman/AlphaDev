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

    using AppDev.Core;

    using FluentAssertions;
    using FluentAssertions.Common;

    using LightBDD.XUnit2;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Omego.Extensions.QueryableExtensions;

    using OpenQA.Selenium;

    using Xunit;
    using Xunit.Abstractions;

    using Blog = AlphaDev.Core.Data.Entities.Blog;

    public partial class Homepage_feature : FeatureFixture, IClassFixture<SiteTester>, IDisposable
    {
        private readonly SiteTester siteTester;

        private readonly DatabaseFixture databaseFixture;

        public Homepage_feature(ITestOutputHelper output, SiteTester siteTester)
            : base(output)
        {
            this.siteTester = siteTester;
            databaseFixture = new DatabaseFixture();

            databaseFixture.BlogContext.Blogs.AddRange(
                new Blog
                    {
                        Content = "Content integration test1.",
                        Title = "Title integration test1.",
                        Created = new DateTime(2016, 1, 1)
                    },
                new Blog
                    {
                        Content = "Content integration test2.",
                        Title = "Title integration test2.",
                        Created = new DateTime(2017, 2, 1)
                    });

            databaseFixture.BlogContext.SaveChanges();
        }

        private void Given_i_am_a_user()
        {
        }

        private void When_i_go_to_the_homepage()
        {
            var url = $"http://127.0.0.1:{GetOpenPort()}";
            using (var host = new WebHostBuilder()
                .UseContentRoot(Path.GetFullPath(@"..\..\..\..\..\..\web\AlphaDev.Web")).UseKestrel().ConfigureServices(
                    services =>
                        {
                            services.AddSingleton<IConfigurationBuilder, IConfigurationBuilder>(
                                provider => new ConfigurationBuilder().SetBasePath(Path.GetFullPath(".")));
                        }).UseStartup<Startup>().UseUrls(url).Build())
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

        private void Then_it_should_display_the_latest_blog_post() => new
                                                                          {
                                                                              Title = siteTester.Driver.FindElement(
                                                                                      By.CssSelector("div.blog .title"))
                                                                                  .Text,
                                                                              Content = siteTester.Driver.FindElement(
                                                                                      By.CssSelector(
                                                                                          "div.blog .content"))
                                                                                  .Text
                                                                          }.ShouldBeEquivalentTo(
            databaseFixture.BlogContext.Blogs.OrderByDescending(blog => blog.Created)
                .FirstOrThrow(new InvalidOperationException("No blogs found.")));

        public void Dispose() => databaseFixture.Dispose();
    }
}