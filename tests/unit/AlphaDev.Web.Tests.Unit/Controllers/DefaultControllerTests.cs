﻿using System;
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
    public class DefaultControllerTests
    {
        private DefaultController GetDefaultController()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetLatest().Returns(new Blog(default, null, null, default));

            return GetDefaultController(blogService);
        }

        private DefaultController GetDefaultController(IBlogService blogService)
        {
            return new DefaultController(blogService);
        }

        [Fact]
        public void ErrorShouldReturnErrorView()
        {
            GetDefaultController().Error().Should().BeOfType<ViewResult>().Which.ViewName.Should()
                .BeEquivalentTo("Error");
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
            var blog = new Blog(123,
                "title",
                "content",
                new Dates(new DateTime(2015, 7, 27), Option.Some(new DateTime(2016, 8, 28))));

            var blogService = Substitute.For<IBlogService>();
            blogService.GetLatest().Returns(blog);

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
    }
}