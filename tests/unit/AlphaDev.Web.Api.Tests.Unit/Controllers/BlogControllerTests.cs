using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaDev.BlogServices.Core;
using AlphaDev.Web.Api.Controllers;
using AlphaDev.Web.Api.Models;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.Core;
using Optional;
using Optional.Unsafe;
using Xunit;
using Blog = AlphaDev.BlogServices.Core.Blog;
using Dates = AlphaDev.BlogServices.Core.Dates;

namespace AlphaDev.Web.Api.Tests.Unit.Controllers
{
    public class BlogControllerTests
    {
        [Fact]
        public async Task GetLatestReturnsBlogModelWithValuesSetFromTheBlogService()
        {
            BlogBase blog = new Blog(123,
                "title",
                "content",
                new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var blogService = Substitute.For<IBlogService>();
            blogService.GetLatestAsync().Returns(blog.Some());

            var controller = GetBlogController(blogService);

            (await controller.GetLatest())
                .Should()
                .BeOfType<OkObjectResult>()
                .Which.Value
                .Should()
                .BeEquivalentTo(
                    new
                    {
                        blog.Id,
                        blog.Title,
                        blog.Content,
                        Dates = new { blog.Dates.Created, Modified = blog.Dates.Modified.ValueOrDefault() }
                    });
        }

        [Fact]
        public async Task GetLatestReturnsNotFoundWhenServiceReturnsNone()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetLatestAsync().Returns(Option.None<BlogBase>());

            var controller = GetBlogController(blogService);

            (await controller.GetLatest()).Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetBlogsReturnsBlogsWithTotalFromTheBlogServiceWhenFound()
        {
            var blogService = Substitute.For<IBlogService>();
            var blogs = new []{new Blog(1, "title", "content", new Dates(new DateTime(), DateTime.MaxValue.Some())) };
            var blogsWithCount = (total: 100, blogs);
            blogService.GetOrderedByDatesWithTotalAsync(10, 7)
                       .Returns(blogsWithCount);

            var controller = GetBlogController(blogService);

            var result = await controller.GetBlogs(10, 7);

            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(new
            {
                Values = blogsWithCount.blogs.Select(b =>  new
                {
                    Dates = new
                    {
                        b.Dates.Created,
                        Modified = b.Dates.Modified.ValueOrDefault()
                    },
                    b.Title,
                    b.Content,
                    b.Id
                }),
                Total = blogsWithCount.total
            });
        }

        [Fact]
        public async Task GetReturnsBlogModelWithValuesSetFromTheBlogService()
        {
            BlogBase blog = new Blog(123,
                "title",
                "content",
                new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var blogService = Substitute.For<IBlogService>();
            blogService.GetAsync(1).Returns(blog.Some());

            var controller = GetBlogController(blogService);

            (await controller.Get(1))
                .Should()
                .BeOfType<OkObjectResult>()
                .Which.Value
                .Should()
                .BeEquivalentTo(
                    new
                    {
                        blog.Id,
                        blog.Title,
                        blog.Content,
                        Dates = new { blog.Dates.Created, Modified = blog.Dates.Modified.ValueOrDefault() }
                    });
        }

        [Fact]
        public async Task GetReturnsNotFoundWhenServiceReturnsNone()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetAsync(1).Returns(Option.None<BlogBase>());

            var controller = GetBlogController(blogService);

            (await controller.Get(1)).Should().BeOfType<NotFoundResult>();
        }

        [NotNull]
        private static BlogController GetBlogController(IBlogService blogService) => new BlogController(blogService);
    }
}
