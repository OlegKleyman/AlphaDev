using System;
using System.Globalization;
using System.Linq;
using AlphaDev.Web.Tests.Integration.Fixtures;
using AlphaDev.Web.Tests.Integration.Support;
using FluentAssertions;
using Markdig;
using Omego.Extensions.DbContextExtensions;
using Omego.Extensions.EnumerableExtensions;
using Omego.Extensions.QueryableExtensions;
using Optional;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    public partial class Homepage_feature : WebFeatureFixture
    {
        public Homepage_feature(ITestOutputHelper output, DatabaseWebServerFixture databaseWebServerFixture)
            : base(output, databaseWebServerFixture)
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
                DatabaseFixture.BlogContext.Blogs.OrderByDescending(blog => blog.Created).ToList().Select(blog => new
                    {
                        Dates = new
                        {
                            Created = blog.Created.ToString(FullDateFormatString, CultureInfo.InvariantCulture)
                        }
                    })
                    .FirstOrThrow(new InvalidOperationException("No blogs found.")),
                options => options.Including(post => post.Dates.Created));
        }

        private void And_the_latest_blog_post_was(ModifiedState modifiedState)
        {
            var modified = modifiedState == ModifiedState.Modified ? new DateTime(2017, 7, 12) : (DateTime?) null;

            DatabaseFixture.BlogContext.Blogs.OrderByDescending(blog => blog.Created).First().Modified = modified;

            DatabaseFixture.BlogContext.SaveChanges();
        }

        private void Then_it_should_display_the_blog_with_a_modification_date_if_it_exists()
        {
            SiteTester.HomePage.LatestBlog.ShouldBeEquivalentTo(
                DatabaseFixture.BlogContext.Blogs.ToList().Select(blog => new
                {
                    Dates = new
                    {
                        Modified = blog.Modified.ToOption().FlatMap(time =>
                            Option.Some(time.ToString(FullDateFormatString, CultureInfo.InvariantCulture)))
                    }
                }).FirstOrThrow(new InvalidOperationException("No blogs found.")),
                options => options.ExcludingMissingMembers());
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

        private void Then_it_should_display_blog_post_with_markdown_parsed_to_html()
        {
            SiteTester.HomePage.LatestBlog.ShouldBeEquivalentTo(
                DatabaseFixture.BlogContext.Blogs.OrderByDescending(blog => blog.Created).ToList().Select(blog => new
                    {
                        Content = Markdown.ToHtml(blog.Content).Trim()
                    })
                    .FirstOrThrow(new InvalidOperationException("No blogs found.")),
                options => options.Including(post => post.Content));
        }

        private void And_the_latest_blog_post_contains_markdown()
        {
            var blog = DatabaseFixture.DefaultBlog;
            blog.Content = "Content integration `<test2>testing</test2>`.";

            DatabaseFixture.BlogContext.AddRangeAndSave(
                blog);
        }

        private void And_there_are_multiple_blog_posts_at_different_times()
        {
            var blogs = DatabaseFixture.DefaultBlogs;
            blogs[0].Created = DateTime.MinValue;
            blogs[1].Created = DateTime.MaxValue;

            DatabaseFixture.BlogContext.AddRangeAndSave(
                blogs);
        }

        private void And_there_is_a_blog_post_with_single_digit_days()
        {
            var blog = DatabaseFixture.DefaultBlog;
            blog.Created = new DateTime(2017, 2, 1);
            blog.Modified = new DateTime(2017, 7, 8);

            DatabaseFixture.BlogContext.AddRangeAndSave(blog);
        }

        private void Then_it_should_display_two_digits_for_day_for_created()
        {
            SiteTester.HomePage.LatestBlog.Dates.Created.Should().MatchRegex(FullDateFormatRegularExpression);
        }

        private void And_it_should_display_two_digits_for_day_for_modified()
        {
            SiteTester.HomePage.LatestBlog.Dates.Modified
                .ValueOr(() => throw new InvalidOperationException("No modified date found.")).Should()
                .MatchRegex(FullDateFormatRegularExpression);
        }

        private void And_there_is_a_blog_post()
        {
            DatabaseFixture.BlogContext.AddRangeAndSave(
                DatabaseFixture.DefaultBlog);
        }

        private void Then_it_should_display_title()
        {
            SiteTester.HomePage.LatestBlog.ShouldBeEquivalentTo(
                DatabaseFixture.BlogContext.Blogs
                    .FirstOrThrow(new InvalidOperationException("No blogs found.")),
                options => options.Including(post => post.Title));
        }
    }
}