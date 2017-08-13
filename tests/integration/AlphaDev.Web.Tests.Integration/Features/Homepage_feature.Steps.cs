using System;
using System.Linq;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Web.Tests.Integration.Fixtures;
using FluentAssertions;
using Omego.Extensions.QueryableExtensions;
using OpenQA.Selenium;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    public partial class Homepage_feature : WebFeatureFixture, IDisposable
    {
        private readonly DatabaseFixture _databaseFixture;
        private readonly WebServer _webServer;

        public Homepage_feature(ITestOutputHelper output, SiteTester siteTester)
            : base(output, siteTester)
        {
            _databaseFixture = new DatabaseFixture();
            _webServer = new WebServer();

            _databaseFixture.BlogContext.Blogs.AddRange(
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

            _databaseFixture.BlogContext.SaveChanges();

            _webServer.Start(_databaseFixture.ConnectionString);
        }

        public void Dispose()
        {
            _webServer?.Dispose();
            _databaseFixture?.Dispose();
        }

        private void Given_i_am_a_user()
        {
        }

        private void When_i_go_to_the_homepage()
        {
            SiteTester.Driver.Navigate().GoToUrl(_webServer.Url);
        }

        private void Then_it_should_load()
        {
            SiteTester.Driver.Title.ShouldBeEquivalentTo("Home - AlphaDev");
        }

        private void Then_it_should_display_navigation_links()
        {
            SiteTester.Driver.FindElements(By.CssSelector("ul.navbar-nav a"))
                .Select(element => element.Text).ShouldBeEquivalentTo(new[] {"Posts", "About", "Contact"});
        }

        private void Then_it_should_display_the_latest_blog_post()
        {
            new
            {
                Title = SiteTester.Driver.FindElement(
                        By.CssSelector(
                            "div.blog .title h2"))
                    .Text,
                Content = SiteTester.Driver.FindElement(
                        By.CssSelector(
                            "div.blog .content"))
                    .Text,
                Dates = SiteTester.Driver.FindElement(
                    By.CssSelector(
                        "div.blog .dates")).Text
            }.ShouldBeEquivalentTo(
                _databaseFixture.BlogContext.Blogs.OrderByDescending(blog => blog.Created).Select(blog => new
                    {
                        blog.Title,
                        blog.Content,
                        Dates =
                        $"Created: {blog.Created:D} Modified: {(blog.Modified.HasValue ? blog.Modified.Value.ToString("D") : string.Empty)}"
                    })
                    .FirstOrThrow(new InvalidOperationException("No blogs found.")));
        }

        private void And_the_latest_blog_post_was(bool modifiedState)
        {
            var modified = modifiedState ? new DateTime(2017, 7, 12) : (DateTime?) null;

            _databaseFixture.BlogContext.Blogs.OrderByDescending(blog => blog.Created).First().Modified = modified;

            _databaseFixture.BlogContext.SaveChanges();
        }

        private void Then_it_should_display_the_latest_blog_post_with_modification_date(bool modifiedState)
        {
            var dates = SiteTester.Driver.FindElement(By.CssSelector("div.blog .dates")).Text;

            if (modifiedState) dates.Should().Contain("Modified");
            else dates.Should().NotContain("Modified");
        }

        private void Given_the_website_has_a_problem()
        {
            _databaseFixture.BlogContext.Database.EnsureDeleted();
        }

        private void Then_it_should_display_an_error()
        {
            SiteTester.Driver.Title.ShouldBeEquivalentTo("Error - AlphaDev");
        }

        private void Then_an_error_should_be_logged()
        {
            _webServer.Log.Should().Contain("[Error] An unhandled exception has occurred");
        }
    }
}