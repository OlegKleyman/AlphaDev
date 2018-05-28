using System;
using System.Globalization;
using System.Linq;
using AlphaDev.Web.Tests.Integration.Fixtures;
using AlphaDev.Web.Tests.Integration.Support;
using FluentAssertions;
using JetBrains.Annotations;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Extended;
using Markdig;
using Omego.Extensions.DbContextExtensions;
using Omego.Extensions.EnumerableExtensions;
using Optional;
using Optional.Collections;
using Optional.Unsafe;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [UsedImplicitly]
    public partial class Post_feature : WebFeatureFixture
    {
        private int? _postId;
        private string _title;

        public Post_feature(ITestOutputHelper output, DatabaseWebServerFixture databaseWebServerFixture) : base(output,
            databaseWebServerFixture)
        {
        }

        private void When_I_view_it_by_id()
        {
            SiteTester.Posts.GoTo(_postId.ToOption()
                .ValueOr(() => throw new InvalidOperationException("_postId was not initialized.")));
        }

        private void Then_it_should_load_the_page()
        {
            SiteTester.Posts.Title.Should().BeEquivalentTo($"{_title} - AlphaDev");
        }

        private void Given_there_is_a_blog_post(string title)
        {
            var defaultBlog = BlogContextDatabaseFixture.DefaultBlog;
            defaultBlog.Title = title;

            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.AddRangeAndSave(defaultBlog);

            var blog = DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs.FirstOrDefault().SomeNotNull()
                .ValueOr(() => throw new InvalidOperationException("No blogs found"));

            _postId = blog.Id;
            _title = blog.Title;
        }

        private CompositeStep When_I_view_a_post_by_id()
        {
            return CompositeStep.DefineNew()
                .AddSteps(_ => Given_there_is_a_blog_post("testing"), _ => When_I_view_it_by_id()).Build();
        }

        private void Then_it_should_display_the_title()
        {
            SiteTester.Posts.Post.Title.Should()
                .BeEquivalentTo(DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs.Find(_postId).Title);
        }

        private void Then_it_should_display_blog_post_with_markdown_parsed_to_html()
        {
            SiteTester.Posts.Post.Content.Should().BeEquivalentTo(DatabasesFixture.BlogContextDatabaseFixture
                .BlogContext.Blogs
                .OrderByDescending(blog => blog.Created).ToList().Select(blog => Markdown.ToHtml(blog.Content).Trim())
                .FirstOrThrow(new InvalidOperationException("No blogs found.")));
        }

        private void And_the_blog_post_was(ModifiedState modifiedState)
        {
            var modified = modifiedState == ModifiedState.Modified ? new DateTime(2017, 7, 12) : (DateTime?) null;

            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs.Find(_postId).Modified = modified;

            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.SaveChanges();
        }

        private void Then_it_should_display_the_blog_with_a_modification_date_if_it_exists()
        {
            SiteTester.Posts.Post.Dates.Modified.Should().BeEquivalentTo(DatabasesFixture.BlogContextDatabaseFixture
                .BlogContext.Blogs.Find(_postId)
                .Modified.ToOption().FlatMap(time =>
                    Option.Some(time.ToString(FullDateFormatString, CultureInfo.InvariantCulture))));
        }

        private void Then_it_should_display_the_created_date()
        {
            SiteTester.Posts.Post.Dates.Created.Should().BeEquivalentTo(DatabasesFixture.BlogContextDatabaseFixture
                .BlogContext.Blogs.Find(_postId)
                .Created.ToString(FullDateFormatString, CultureInfo.InvariantCulture));
        }

        private void When_I_try_to_view_a_post_that_doesnt_exist()
        {
            SiteTester.Posts.GoTo(1000);
        }

        private void Then_it_should_display_the_posts_menu_link_to_lead_to_all_posts()
        {
            var homePageNavigation = SiteTester.Posts.Navigation.ToList();
            homePageNavigation.FirstOrNone(element => element is AnchorElement && element.Text == "Posts")
                .Map(element => new Uri(((AnchorElement) element).Href).AbsoluteUri).ValueOrFailure()
                .Should().BeEquivalentTo(
                    SiteTester.Posts.BaseUrl.AbsoluteUri.Trim('/') /* trim trailing forward slash(/) */);
        }
    }
}