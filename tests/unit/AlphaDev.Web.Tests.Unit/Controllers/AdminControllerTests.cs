using System;
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
    public class AdminControllerTests
    {
        private AdminController GetAdminController()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetLatest().Returns(((BlogBase) new Blog(default, null, null, default)).Some());

            return GetAdminController(blogService);
        }

        private AdminController GetAdminController(IBlogService blogService)
        {
            return new AdminController();
        }

        [Fact]
        public void IndexShouldReturnIndexView()
        {
            var controller = GetAdminController();

            controller.Index().Should().BeOfType<ViewResult>();
        }
    }
}