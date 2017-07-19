using System.Collections.Generic;

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

    using Optional;

    using Xunit;
    using Xunit.Abstractions;

    using Blog = AlphaDev.Core.Data.Entities.Blog;

    public partial class Homepage_feature : FeatureFixture, IClassFixture<SiteTester>, IDisposable
    {
        private readonly SiteTester siteTester;

        private readonly DatabaseFixture databaseFixture;
        private string connectionString;
        private WebSite webSite;

        public Homepage_feature(ITestOutputHelper output, SiteTester siteTester)
            : base(output)
        {
            this.siteTester = siteTester;
            databaseFixture = new DatabaseFixture();
            webSite = new WebSite();

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
                        Created = new DateTime(2017, 2, 1),
                        Modified = new DateTime(2017, 7, 12)
                });

            databaseFixture.BlogContext.SaveChanges();

            webSite.Start(databaseFixture.ConnectionString);

            connectionString = databaseFixture.ConnectionString;
        }

        private void Given_i_am_a_user()
        {
        }

        private void When_i_go_to_the_homepage()
        {
            siteTester.Driver.Navigate().GoToUrl(webSite.Url);
        }

        private void Then_it_should_load() => siteTester.Driver.Title.ShouldBeEquivalentTo("Home - AlphaDev");

        private void Then_it_should_display_navigation_links() => siteTester.Driver.FindElements(By.CssSelector("ul.navbar-nav a"))
            .Select(element => element.Text).ShouldBeEquivalentTo(new[] { "Posts", "About", "Contact" });

        private void Then_it_should_display_the_latest_blog_post()
        {
            new
            {
                Title = siteTester.Driver.FindElement(
                        By.CssSelector(
                            "div.blog .title h2"))
                    .Text,
                Content = siteTester.Driver.FindElement(
                        By.CssSelector(
                            "div.blog .content"))
                    .Text,
                Dates = siteTester.Driver.FindElement(
                    By.CssSelector(
                        "div.blog .dates")).Text
            }.ShouldBeEquivalentTo(
                databaseFixture.BlogContext.Blogs.OrderByDescending(blog => blog.Created).Select(blog => new
                    {
                        blog.Title,
                        blog.Content,
                        Dates =
                        $"Created: {blog.Created:D} Modified: {(blog.Modified.HasValue ? blog.Modified.Value.ToString("D") : string.Empty)}"
                    })
                    .FirstOrThrow(new InvalidOperationException("No blogs found.")));
        }

        public void Dispose()
        {
            webSite?.Dispose();
            databaseFixture?.Dispose();
        }

        private void And_the_latest_blog_post_was(bool modifiedState)
        {
            var modified = modifiedState ? new DateTime(2017, 7, 12) : (DateTime?)null;

            databaseFixture.BlogContext.Blogs.OrderByDescending(blog => blog.Created).First().Modified = modified;

            databaseFixture.BlogContext.SaveChanges();
        }

        private void Then_it_should_display_the_latest_blog_post_with_modification_date(bool modifiedState)
        {
            var dates = siteTester.Driver.FindElement(By.CssSelector("div.blog .dates")).Text;

            if (modifiedState) dates.Should().Contain("Modified");
            else dates.Should().NotContain("Modified");
        }

        private void Given_the_website_has_a_problem()
        {
            databaseFixture.BlogContext.Database.EnsureDeleted();
        }

        private void Then_it_should_display_an_error()
        {
            siteTester.Driver.Title.ShouldBeEquivalentTo("Error - AlphaDev");
        }
    }
}