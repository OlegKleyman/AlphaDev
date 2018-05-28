using System;
using AlphaDev.Core;
using AlphaDev.Web.Controllers;
using AlphaDev.Web.Models;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Optional;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Controllers
{
    public class DefaultControllerTests
    {
        [NotNull]
        private DefaultController GetDefaultController()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetLatest().Returns(((BlogBase) new Blog(default, null, null, default)).Some());

            return GetDefaultController(blogService);
        }

        [NotNull]
        private DefaultController GetDefaultController([NotNull] IBlogService blogService)
        {
            return new DefaultController(blogService);
        }

        [Fact]
        public void ErrorShouldReturnErrorModelWithErrorMessage()
        {
            GetDefaultController().Error(500).Should().BeOfType<ViewResult>().Which.Model.Should()
                .BeOfType<ErrorModel>().Which.Message.Should()
                .BeEquivalentTo("An error occurred while processing your request.");
        }

        [Fact]
        public void ErrorShouldReturnErrorModelWithStatus()
        {
            GetDefaultController().Error(500).Should().BeOfType<ViewResult>().Which.Model.Should()
                .BeOfType<ErrorModel>().Which.Status.Should().Be(500);
        }

        [Fact]
        public void ErrorShouldReturnErrorView()
        {
            GetDefaultController().Error(default).Should().BeOfType<ViewResult>().Which.ViewName.Should()
                .BeEquivalentTo("Error");
        }

        [Fact]
        public void ErrorShouldReturnWithErrorModel()
        {
            GetDefaultController().Error(default).Should().BeOfType<ViewResult>().Which.Model.Should()
                .BeOfType<ErrorModel>();
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
            BlogBase blog = new Blog(123,
                "title",
                "content",
                new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var blogService = Substitute.For<IBlogService>();
            blogService.GetLatest().Returns(blog.Some());

            var controller = GetDefaultController(blogService);

            controller.Index().Model.Should().BeEquivalentTo(
                new {blog.Id, blog.Title, blog.Content, Dates = new {blog.Dates.Created, blog.Dates.Modified}});
        }

        [Fact]
        public void IndexShouldReturnIndexView()
        {
            var controller = GetDefaultController();

            controller.Index().Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void IndexShouldReturnWelcomeBlogViewModelIfNoBlogsExist()
        {
            var service = Substitute.For<IBlogService>();
            var controller = GetDefaultController(service);

            controller.Index().Model.Should().BeEquivalentTo(
                BlogViewModel.Welcome);
        }
    }
}