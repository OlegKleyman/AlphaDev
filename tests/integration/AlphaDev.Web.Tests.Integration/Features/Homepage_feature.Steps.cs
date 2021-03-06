﻿using System;
using System.Globalization;
using System.Linq;
using AlphaDev.Web.Tests.Integration.Fixtures;
using AlphaDev.Web.Tests.Integration.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Markdig;
using Omego.Extensions.DbContextExtensions;
using Omego.Extensions.EnumerableExtensions;
using Omego.Extensions.QueryableExtensions;
using Optional;
using Optional.Unsafe;
using Polly;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [UsedImplicitly]
    public partial class Homepage_feature : WebFeatureFixture
    {
        public Homepage_feature(ITestOutputHelper output, [NotNull] DatabaseWebServerFixture databaseWebServerFixture)
            : base(output, databaseWebServerFixture)
        {
        }

        private void When_i_go_to_the_homepage()
        {
            SiteTester.HomePage.GoTo();
        }

        private void Then_it_should_load()
        {
            SiteTester.HomePage.Title.Should().BeEquivalentTo("Home - AlphaDev");
        }

        private void Then_it_should_display_navigation_links()
        {
            SiteTester.HomePage.Navigation.Select(element => element.Text)
                      .Should()
                      .BeEquivalentTo("Posts", "About", "Contact");
        }

        private void Then_it_should_display_the_latest_blog_post()
        {
            SiteTester.HomePage.LatestBlog.ValueOrFailure()
                      .Should()
                      .BeEquivalentTo(
                          DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs
                                          .OrderByDescending(blog => blog.Created)
                                          .ToList()
                                          .Select(blog => new
                                          {
                                              Dates = new
                                              {
                                                  Created = blog.Created.ToString(FullDateFormatString,
                                                      CultureInfo.InvariantCulture)
                                              }
                                          })
                                          .FirstOrThrow(new InvalidOperationException("No blogs found.")),
                          options => options.Including(post => post.Dates.Created));
        }

        private void And_the_latest_blog_post_was(ModifiedState modifiedState)
        {
            var modified = modifiedState == ModifiedState.Modified ? new DateTime(2017, 7, 12) : (DateTime?) null;

            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs.OrderByDescending(blog => blog.Created)
                            .First()
                            .Modified = modified;

            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.SaveChanges();
        }

        private void Then_it_should_display_the_blog_with_a_modification_date_if_it_exists()
        {
            SiteTester.HomePage.LatestBlog.ValueOrFailure()
                      .Should()
                      .BeEquivalentTo(
                          DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs
                                          .OrderByDescending(blog => blog.Created)
                                          .ToList()
                                          .Select(blog => new
                                          {
                                              Dates = new
                                              {
                                                  Modified = blog.Modified.ToOption()
                                                                 .FlatMap(time =>
                                                                     Option.Some(time.ToString(FullDateFormatString,
                                                                         CultureInfo.InvariantCulture)))
                                              }
                                          })
                                          .FirstOrThrow(new InvalidOperationException("No blogs found.")),
                          options => options.ExcludingMissingMembers());
        }

        private void Given_the_website_has_a_problem()
        {
            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Database.EnsureDeleted();
        }

        private void Then_it_should_display_an_error()
        {
            SiteTester.Driver.Title.Should().BeEquivalentTo("Error - AlphaDev");
        }

        private void Then_an_error_should_be_logged()
        {
            Policy.HandleResult<string>(s => !s.Contains("[Error] An unhandled exception has occurred"))
                  .WaitAndRetry(11, i => TimeSpan.FromMilliseconds(100), (_, __, time, ___) =>
                  {
                      if (time == 11)
                      {
                          throw new InvalidOperationException(
                              "Log does not contain unhandled error message.");
                      }
                  })
                  .Execute(() => Log);
        }

        private void Then_it_should_display_blog_post_with_markdown_parsed_to_html()
        {
            SiteTester.HomePage.LatestBlog.ValueOrFailure()
                      .Should()
                      .BeEquivalentTo(
                          DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs
                                          .OrderByDescending(blog => blog.Created)
                                          .ToList()
                                          .Select(blog => new
                                          {
                                              Content = Markdown.ToHtml(blog.Content).Trim()
                                          })
                                          .FirstOrThrow(new InvalidOperationException("No blogs found.")),
                          options => options.Including(post => post.Content));
        }

        private void And_the_latest_blog_post_contains_markdown()
        {
            var blog = BlogContextDatabaseFixture.DefaultBlog;
            blog.Content = "Content integration `<test2>testing</test2>`.";

            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.AddRangeAndSave(
                blog);
        }

        private void And_there_are_multiple_blog_posts_at_different_times()
        {
            var blogs = BlogContextDatabaseFixture.DefaultBlogs;
            blogs[0].Created = DateTime.MinValue;
            blogs[1].Created = DateTime.MaxValue;

            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.AddRangeAndSave(
                // avoid co-variant conversion
                blogs.Select(blog => (object) blog).ToArray());
        }

        private void And_there_is_a_blog_post_with_single_digit_days()
        {
            var blog = BlogContextDatabaseFixture.DefaultBlog;
            blog.Created = new DateTime(2017, 2, 1);
            blog.Modified = new DateTime(2017, 7, 8);

            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.AddRangeAndSave(blog);
        }

        private void Then_it_should_display_two_digits_for_day_for_created()
        {
            SiteTester.HomePage.LatestBlog.ValueOrFailure()
                      .Dates.Created.Should()
                      .MatchRegex(FullDateFormatRegularExpression);
        }

        private void And_it_should_display_two_digits_for_day_for_modified()
        {
            SiteTester.HomePage.LatestBlog.ValueOrFailure()
                      .Dates.Modified
                      .ValueOr(() => throw new InvalidOperationException("No modified date found."))
                      .Should()
                      .MatchRegex(FullDateFormatRegularExpression);
        }

        private void And_there_is_a_blog_post()
        {
            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.AddRangeAndSave(
                BlogContextDatabaseFixture.DefaultBlog);
        }

        private void Then_it_should_display_title()
        {
            SiteTester.HomePage.LatestBlog.ValueOrFailure()
                      .Should()
                      .BeEquivalentTo(
                          DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs
                                          .FirstOrThrow(new InvalidOperationException("No blogs found.")),
                          options => options.Including(post => post.Title));
        }

        private void And_there_are_no_blog_posts()
        {
            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs.RemoveRange(DatabasesFixture
                                                                                      .BlogContextDatabaseFixture
                                                                                      .BlogContext.Blogs);
        }

        private void Then_it_should_display_welcome_post()
        {
            const string content = "<pre class=\" language-csharp\"><code class=\" language-csharp\">" +
                                   "<span class=\"token keyword\">public</span> <span class=\"token keyword\">" +
                                   "void</span> <span class=\"token function\">Main</span><span " +
                                   "class=\"token punctuation\">(</span><span class=\"token punctuation\">)" +
                                   "</span>\r\n<span class=\"token punctuation\">{</span>\r\n\t\tConsole" +
                                   "<span class=\"token punctuation\">.</span><span class=\"token function\">" +
                                   "Writeline</span><span class=\"token punctuation\">(</span><span " +
                                   "class=\"token string\">\"Hello\"</span><span class=\"token punctuation\">)" +
                                   "</span><span class=\"token punctuation\">;</span>\r\n<span " +
                                   "class=\"token punctuation\">}</span>\r\n</code></pre>";

            SiteTester.HomePage.LatestBlog.ValueOrFailure()
                      .Should()
                      .BeEquivalentTo(
                          new
                          {
                              Title = "Welcome to my blog.",
                              Content = content,
                              Dates = new
                              {
                                  Created = "Monday, January 01, 0001",
                                  Modified = Option.None<string>()
                              }
                          }
                      );
        }
    }
}