using System;
using System.Collections.Generic;
using System.Linq;
using AlphaDev.Core;
using AlphaDev.Web.Controllers;
using AlphaDev.Web.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Optional;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Controllers
{
    public class PostsControllerTests
    {
        private PostsController GetPostsController()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetLatest().Returns(new Blog(default(int), null, null, default(Dates)));

            return GetPostsController(blogService);
        }

        private PostsController GetPostsController(IBlogService blogService)
        {
            return new PostsController(blogService);
        }

        [Fact]
        public void IndexShouldReturnBlogModelWithValuesSetFromTheBlogService()
        {
            var blog = new Blog(123,
                "title",
                "content",
                new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var blogService = Substitute.For<IBlogService>();
            blogService.GetAll().Returns(new[]{blog});

            var controller = GetPostsController(blogService);

            controller.Index().Model.ShouldBeEquivalentTo(
                new[] {new {blog.Id, blog.Title, blog.Content, Dates = new {blog.Dates.Created, blog.Dates.Modified}}});
        }

        [Fact]
        public void IndexShouldReturnIndexView()
        {
            var controller = GetPostsController();

            controller.Index().Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void IndexShouldReturnBlogModelOrderedByCreatedDateDescending()
        {
            var blogs = new[]
            {
                new Blog(321, default(string), default(string), new Dates(new DateTime(2014,1,1), Option.None<DateTime>())),
                new Blog(123,
                    "title",
                    "content",
                    new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))))
            };

            var blogService = Substitute.For<IBlogService>();
            blogService.GetAll().Returns(blogs);

            var controller = GetPostsController(blogService);

            controller.Index().Model.Should().BeAssignableTo<IEnumerable<BlogViewModel>>().Which.Should()
                .BeInDescendingOrder(model => model.Dates.Created);
        }

        [Fact]
        public void IndexShouldReturnBlogModelAssignableToEnumerableOfBlogViewModel()
        {
            var blogs = new[]
            {
                new Blog(321, default(string), default(string), new Dates(new DateTime(2014,1,1), Option.None<DateTime>())),
                new Blog(123,
                    "title",
                    "content",
                    new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))))
            };

            var blogService = Substitute.For<IBlogService>();
            blogService.GetAll().Returns(blogs);

            var controller = GetPostsController(blogService);

            controller.Index().Model.Should().BeAssignableTo<IEnumerable<BlogViewModel>>();
        }

    }
}