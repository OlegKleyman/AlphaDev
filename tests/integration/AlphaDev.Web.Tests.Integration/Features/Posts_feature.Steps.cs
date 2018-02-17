﻿using System;
using System.Globalization;
using System.Linq;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Web.Tests.Integration.Fixtures;
using AlphaDev.Web.Tests.Integration.Support;
using FluentAssertions;
using LightBDD.Framework.Scenarios.Basic;
using Markdig;
using Omego.Extensions.DbContextExtensions;
using Omego.Extensions.EnumerableExtensions;
using Optional;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    public partial class Posts_feature : WebFeatureFixture
    {
        public Posts_feature(ITestOutputHelper output, DatabaseWebServerFixture databaseWebServerFixture) : base(output, databaseWebServerFixture)
        {
        }
        
        private void When_i_go_to_the_posts_page()
        {
            SiteTester.Posts.GoTo();
        }

        private void Then_it_should_load()
        {
            SiteTester.Posts.Title.ShouldBeEquivalentTo("Posts - AlphaDev");
        }

        private void And_there_are_multiple_posts()
        {
            DatabaseFixture.BlogContext.AddRangeAndSave(DatabaseFixture.DefaultBlogs);
        }

        private void Then_it_should_display_all_posts()
        {
            SiteTester.Posts.Posts.Should().HaveSameCount(DatabaseFixture.BlogContext.Blogs);
        }

        private void Then_it_should_display_all_posts_ordered_by_creation_date_descending()
        {
            SiteTester.Posts.Posts.Select(post => DateTime.Parse(post.Dates.Created)).Should()
                .BeInDescendingOrder(createdDate => createdDate);
        }

        private void Then_it_should_display_all_posts_with_markdown_parsed_to_html()
        {
            SiteTester.Posts.Posts.ShouldBeEquivalentTo(
                DatabaseFixture.BlogContext.Blogs.OrderByDescending(blog => blog.Created).ToList().Select(blog => new
                    {
                        Content = Markdown.ToHtml(blog.Content).Trim()
                    }),
                options => options.ExcludingMissingMembers());
        }

        private void And_there_are_multiple_posts_with_markdown()
        {
            var blogs = DatabaseFixture.DefaultBlogs;

            for (var i = 0; i < blogs.Length; i++)
            {
                blogs[i].Content = $"Content integration `<test{i}>testing</test{i}>`.";
            }
            
            DatabaseFixture.BlogContext.AddRangeAndSave(
                blogs);
        }

        private void Then_it_should_display_all_posts_with_modification_date_if_it_exists()
        {
            SiteTester.Posts.Posts.ShouldBeEquivalentTo(
                DatabaseFixture.BlogContext.Blogs.ToList().Select(blog => new
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
            SiteTester.Posts.Posts.ShouldBeEquivalentTo(
                DatabaseFixture.BlogContext.Blogs.ToList().Select(blog => new
                {
                    blog.Title
                }),
                options => options.ExcludingMissingMembers());
        }

        private void And_all_posts_were(ModifiedState modifiedState)
        {
            var blogs = DatabaseFixture.DefaultBlogs;

            for (var i = 0; i < blogs.Length; i++)
            {
                blogs[i].Modified = modifiedState == ModifiedState.Modified ? new DateTime(2017, 7, i + 1) : (DateTime?)null;
            }

            DatabaseFixture.BlogContext.AddRangeAndSave(
                blogs);
        }
    }
}
