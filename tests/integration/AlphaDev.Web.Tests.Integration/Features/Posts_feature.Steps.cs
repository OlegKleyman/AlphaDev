using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using AlphaDev.Web.Tests.Integration.Extensions;
using AlphaDev.Web.Tests.Integration.Fixtures;
using AlphaDev.Web.Tests.Integration.Support;
using FluentAssertions;
using JetBrains.Annotations;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.Framework.Scenarios.Extended;
using Markdig;
using Omego.Extensions.DbContextExtensions;
using Optional;
using Optional.Collections;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [UsedImplicitly]
    public partial class Posts_feature : WebFeatureFixture
    {
        private int _pages;
        private int _pageToNavigateTo;

        public Posts_feature(ITestOutputHelper output, [NotNull] DatabaseWebServerFixture databaseWebServerFixture) : base(output,
            databaseWebServerFixture)
        {
        }

        private void When_i_go_to_the_posts_page()
        {
            SiteTester.Posts.GoTo();
        }

        private void Then_it_should_load()
        {
            SiteTester.Posts.Title.Should().BeEquivalentTo("Posts - AlphaDev");
        }

        private void And_there_are_multiple_posts()
        {
            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.AddRangeAndSave(BlogContextDatabaseFixture
                .DefaultBlogs);
        }

        private void Then_it_should_display_all_posts()
        {
            SiteTester.Posts.Posts.Should()
                .HaveSameCount(DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs);
        }

        private void Then_it_should_display_all_posts_ordered_by_creation_date_descending()
        {
            SiteTester.Posts.Posts.Select(post => DateTime.Parse(post.Dates.Created)).Should()
                .BeInDescendingOrder(createdDate => createdDate);
        }

        private void Then_it_should_display_all_posts_with_markdown_parsed_to_html()
        {
            SiteTester.Posts.Posts.Should().BeEquivalentTo(
                DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs.OrderByDescending(blog => blog.Created)
                    .ToList().Select(blog => new
                    {
                        Content = Markdown.ToHtml(blog.Content).Trim()
                    }),
                options => options.ExcludingMissingMembers());
        }

        private void And_there_are_multiple_posts_with_markdown()
        {
            var blogs = BlogContextDatabaseFixture.DefaultBlogs;

            for (var i = 0; i < blogs.Length; i++)
                blogs[i].Content = $"Content integration `<test{i}>testing</test{i}>`.";

            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.AddRangeAndSave(
                blogs);
        }

        private void Then_it_should_display_all_posts_with_modification_date_if_it_exists()
        {
            SiteTester.Posts.Posts.Should().BeEquivalentTo(
                DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs.ToList().Select(blog => new
                {
                    Dates = new
                    {
                        Modified = blog.Modified.ToOption().FlatMap(time =>
                            Option.Some(time.ToString(FullDateFormatString, CultureInfo.InvariantCulture)))
                    }
                }),
                options => options.ExcludingMissingMembers());
        }

        private void Then_it_should_display_all_posts_with_a_title()
        {
            SiteTester.Posts.Posts.Should().BeEquivalentTo(
                DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs.ToList().Select(blog => new
                {
                    blog.Title
                }),
                options => options.ExcludingMissingMembers());
        }

        private void And_all_posts_were(ModifiedState modifiedState)
        {
            var blogs = BlogContextDatabaseFixture.DefaultBlogs;

            for (var i = 0; i < blogs.Length; i++)
                blogs[i].Modified = modifiedState == ModifiedState.Modified
                    ? new DateTime(2017, 7, i + 1)
                    : (DateTime?) null;

            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.AddRangeAndSave(
                blogs);
        }

        private void Then_it_should_display_all_posts_with_a_navigation_link_to_the_entire_post()
        {
            SiteTester.Posts.Posts.Should().BeEquivalentTo(
                DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs.ToList().Select(blog => new
                {
                    NavigationLink = new
                    {
                        Href = new Uri(SiteTester.Posts.BaseUrl,
                            blog.Id.ToString(CultureInfo.InvariantCulture)).AbsoluteUri
                    }
                }),
                options => options.ExcludingMissingMembers());
        }

        private void And_there_are_PAGES_pages_of_posts(int pages)
        {
            const int itemsPerPage = 10;
            foreach (var page in Enumerable.Range(1, pages * itemsPerPage))
            {
                var defaultBlog = BlogContextDatabaseFixture.DefaultBlog;
                defaultBlog.Title = page.ToString(CultureInfo.InvariantCulture);
                DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Add(defaultBlog);
            }

            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.SaveChanges();
            _pages = pages;
        }

        private void Then_it_should_display_all_the_pages()
        {
            const int maxPages = 5;
            SiteTester.Posts.Pages.Take(maxPages).Select((x, i) => new { x.Page.Identity.Number, TextFormat = x.Page.DisplayFormat, Position = i + 1 })
                .Should().OnlyContain(x => x.TextFormat == DisplayFormat.Number || _pages == 0).And.Subject
                .Should().HaveCount(Math.Min(_pages, maxPages)).And.Subject.Should()
                .OnlyContain(x => x.Number == x.Position);
        }

        private void Then_it_should_display_ellipses_after_the_last_max_page()
        {
            SiteTester.Posts.Pages.LastOrNone()
                .WithException(() => new InvalidOperationException("No page links found"))
                .Map(x => x.Page.Identity.DisplayValue.IsEllipses().Should()).ValueOr(x => throw x).BeTrue();
        }

        private void Then_it_should_display_the_current_page_as_grayed_out()
        {
            SiteTester.Posts.Pages.Where(x => x.Page == SiteTester.Posts.CurrentPage).Should()
                .ContainSingle().Which.Page.Active.Should().BeTrue();
        }

        [UsedImplicitly]
        private CompositeStep And_there_are_multiple_pages_of_posts()
        {
            return CompositeStep.DefineNew().AddSteps(_ => And_there_are_PAGES_pages_of_posts(7)).Build();
        }

        private void When_i_go_to_the_PAGE_page(int page)
        {
            _pageToNavigateTo = page;
            SiteTester.Posts.Pages.Where(x => x.Page.Identity.Number == page).SingleOrNone()
                .WithException(() => new InvalidOperationException()).Match(x => x.GoTo(), x => throw x);
        }

        private void Then_the_current_page_should_be_the_navigated_to_page()
        {
            SiteTester.Posts.CurrentPage.Should().Be(_pageToNavigateTo);
        }
    }
}