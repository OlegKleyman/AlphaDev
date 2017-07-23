using System;
using AlphaDev.Core.Data.Contexts;
using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
    public class BlogServiceTests
    {
        private BlogService GetBlogService(BlogContext context)
        {
            return new BlogService(context ?? new MockBlogContext("default"));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithContent()
        {
            const string testValue = "test content";

            var context = new MockBlogContext(nameof(GetLatestShouldReturnBlogWithContent));
            context.Blogs.Add(new Data.Entities.Blog {Content = testValue});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetLatest().ShouldBeEquivalentTo(
                new {Content = testValue},
                options => options.Including(info => info.Content));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithCreatedDate()
        {
            var testValue = new DateTime(2017, 1, 1);

            var context = new MockBlogContext(nameof(GetLatestShouldReturnBlogWithCreatedDate));
            context.Blogs.Add(new Data.Entities.Blog {Created = new DateTime(2017, 1, 1)});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetLatest().ShouldBeEquivalentTo(
                new {Dates = new {Created = testValue}},
                options => options.Including(info => info.Dates.Created));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithEmptyContentWhenDbContentIsNull()
        {
            var context = new MockBlogContext(nameof(GetLatestShouldReturnBlogWithEmptyContentWhenDbContentIsNull));
            context.Blogs.Add(new Data.Entities.Blog {Content = null});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetLatest().ShouldBeEquivalentTo(
                new {Content = string.Empty},
                options => options.Including(info => info.Content));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithEmptyTitleWhenDbTitleIsNull()
        {
            var context = new MockBlogContext(nameof(GetLatestShouldReturnBlogWithEmptyTitleWhenDbTitleIsNull));
            context.Blogs.Add(new Data.Entities.Blog {Title = null});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetLatest().ShouldBeEquivalentTo(
                new {Title = string.Empty},
                options => options.Including(info => info.Title));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithModifiedDate()
        {
            var testValue = new DateTime(2017, 1, 1);

            var context = new MockBlogContext(nameof(GetLatestShouldReturnBlogWithModifiedDate));
            context.Blogs.Add(new Data.Entities.Blog {Modified = new DateTime(2017, 1, 1)});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetLatest().ShouldBeEquivalentTo(
                new {Dates = new {Modified = Option.Some(testValue)}},
                options => options.Including(info => info.Dates.Modified));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithNoModifiedDateWhenDbModifiedDateIsNull()
        {
            var context = new MockBlogContext(
                nameof(GetLatestShouldReturnBlogWithNoModifiedDateWhenDbModifiedDateIsNull));
            context.Blogs.Add(new Data.Entities.Blog {Modified = null});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetLatest().ShouldBeEquivalentTo(
                new {Dates = new {Modified = Option.None<DateTime>()}},
                options => options.Including(info => info.Dates.Modified));
        }

        [Fact]
        public void GetLatestShouldReturnBlogWithTitle()
        {
            const string testValue = "test";

            var context = new MockBlogContext(nameof(GetLatestShouldReturnBlogWithTitle));
            context.Blogs.Add(new Data.Entities.Blog {Title = testValue});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetLatest().ShouldBeEquivalentTo(
                new {Title = testValue},
                options => options.Including(info => info.Title));
        }

        [Fact]
        public void GetLatestShouldReturnEmptyBlogWhenNoBlogIsFound()
        {
            var service = GetBlogService(new MockBlogContext(nameof(GetLatestShouldReturnEmptyBlogWhenNoBlogIsFound)));

            service.GetLatest().Should().BeSameAs(BlogBase.Empty);
        }

        [Fact]
        public void GetLatestShouldReturnLatestBlog()
        {
            var context = new MockBlogContext(nameof(GetLatestShouldReturnLatestBlog));
            context.Blogs.AddRange(
                new Data.Entities.Blog {Created = new DateTime(2017, 1, 1)},
                new Data.Entities.Blog {Created = new DateTime(2013, 1, 1)},
                new Data.Entities.Blog {Created = new DateTime(2017, 6, 20)},
                new Data.Entities.Blog {Created = new DateTime(2014, 1, 1)});
            context.SaveChanges();

            var service = GetBlogService(context);

            service.GetLatest().ShouldBeEquivalentTo(
                new {Dates = new {Created = new DateTime(2017, 6, 20)}},
                options => options.Including(info => info.Dates.Created));
        }
    }
}