using System;
using System.Linq;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Web.Tests.Integration.Fixtures;
using FluentAssertions;
using Markdig;
using Omego.Extensions.QueryableExtensions;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    public partial class Homepage_feature : WebFeatureFixture
    {
        public Homepage_feature(ITestOutputHelper output, DatabaseWebServerFixture databaseWebServerFixture)
            : base(output, databaseWebServerFixture)
        {
        }

        private void Given_i_am_a_user()
        {
        }

        private void When_i_go_to_the_homepage()
        {
            SiteTester.HomePage.GoTo();
        }

        private void Then_it_should_load()
        {
            SiteTester.HomePage.Title.ShouldBeEquivalentTo("Home - AlphaDev");
        }

        private void Then_it_should_display_navigation_links()
        {
            SiteTester.HomePage.Navigation.ShouldBeEquivalentTo(new[] {"Posts", "About", "Contact"},
                options => options.Including(info => info.SelectedMemberPath == "Text"));
        }

        private void Then_it_should_display_the_latest_blog_post()
        {
            SiteTester.HomePage.LatestBlog.ShouldBeEquivalentTo(
                DatabaseFixture.BlogContext.Blogs.OrderByDescending(blog => blog.Created).Select(blog => new
                    {
                        blog.Title,
                        Content = Markdown.ToHtml(blog.Content, null).Trim(),
                        Dates =
                        $"Created: {blog.Created:D} Modified: {(blog.Modified.HasValue ? blog.Modified.Value.ToString("D") : string.Empty)}"
                    })
                    .FirstOrThrow(new InvalidOperationException("No blogs found.")));
        }

        private void And_the_latest_blog_post_was(bool modifiedState)
        {
            var modified = modifiedState ? new DateTime(2017, 7, 12) : (DateTime?) null;

            DatabaseFixture.BlogContext.Blogs.OrderByDescending(blog => blog.Created).First().Modified = modified;

            DatabaseFixture.BlogContext.SaveChanges();
        }

        private void Then_it_should_display_the_latest_blog_post_with_modification_date(bool modifiedState)
        {
            var dates = SiteTester.HomePage.LatestBlog.Dates;

            if (modifiedState) dates.Should().Contain("Modified");
            else dates.Should().NotContain("Modified");
        }

        private void Given_the_website_has_a_problem()
        {
            DatabaseFixture.BlogContext.Database.EnsureDeleted();
        }

        private void Then_it_should_display_an_error()
        {
            SiteTester.Driver.Title.ShouldBeEquivalentTo("Error - AlphaDev");
        }

        private void Then_an_error_should_be_logged()
        {
            Log.Should().Contain("[Error] An unhandled exception has occurred");
        }

        private void Then_it_should_display__blog_post_with_markdown_parsed_to_html()
        {
            SiteTester.HomePage.LatestBlog.ShouldBeEquivalentTo(
                DatabaseFixture.BlogContext.Blogs.OrderByDescending(blog => blog.Created).Select(blog => new
                    {
                        blog.Title,
                        Content = Markdown.ToHtml(blog.Content, null).Trim(),
                        Dates =
                        $"Created: {blog.Created:D} Modified: {(blog.Modified.HasValue ? blog.Modified.Value.ToString("D") : string.Empty)}"
                    })
                    .FirstOrThrow(new InvalidOperationException("No blogs found.")));
        }

        private void And_the_latest_blog_post_contains_markdown()
        {
            DatabaseFixture.BlogContext.Blogs.AddRange(
                new Blog
                {
                    Content = "Content integration `<test2>testing</test2>`.",
                    Title = "Title integration test2.",
                    Created = new DateTime(2017, 2, 1),
                    Modified = new DateTime(2017, 7, 12)
                });

            DatabaseFixture.BlogContext.SaveChanges();
        }

        private void And_there_are_multiple_blog_posts_at_different_times()
        {
            DatabaseFixture.BlogContext.Blogs.AddRange(
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

            DatabaseFixture.BlogContext.SaveChanges();
        }
    }
}