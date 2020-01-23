using System;
using System.Threading.Tasks;
using AlphaDev.BlogServices.Core;
using AlphaDev.Web.Api.Controllers;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Optional;
using Optional.Unsafe;
using Xunit;

namespace AlphaDev.Web.Api.Tests.Unit
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
                .Value.Should()
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

            (await controller.GetLatest()).Result.Should().BeOfType<NotFoundResult>();
        }

        [NotNull]
        private BlogController GetBlogController(IBlogService blogService) => new BlogController(blogService);
    }
}
