namespace AlphaDev.Web.Tests.Unit.Controllers
{
    using System;

    using AlphaDev.Web.Controllers;
    using AlphaDev.Web.Models;

    using AppDev.Core;

    using FluentAssertions;
    using FluentAssertions.Mvc;

    using Microsoft.AspNetCore.Mvc;

    using NSubstitute;

    using Optional;

    using Xunit;

    public class DefaultControllerTests
    {
        private DefaultController GetDefaultController()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetLatest().Returns(new Blog(null, null, default(Dates)));

            return GetDefaultController(blogService);
        }

        [Fact]
        public void IndexShouldReturnIndexView()
        {
            var controller = GetDefaultController();

            controller.Index().Should().BeViewResult();
        }

        [Fact]
        public void IndexShouldReturnBlogModel()
        {
            var controller = GetDefaultController();

            controller.Index().Model.Should().BeOfType<BlogViewModel>();
        }

        [Fact]
        public void IndexShouldReturnBlogModelWithValuesSetFromTheBlogService()
        {
            var blog = new Blog(
                "title",
                "content",
                new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var blogService = Substitute.For<IBlogService>();
            blogService.GetLatest().Returns(blog);

            var controller = GetDefaultController(blogService);

            controller.Index().Model.ShouldBeEquivalentTo(
                new { blog.Title, blog.Content, Dates = new { blog.Dates.Created, blog.Dates.Modified } });
        }

        private DefaultController GetDefaultController(IBlogService blogService) => new DefaultController(blogService);
    }
}