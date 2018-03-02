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
            return new AdminController();
        }

        [Fact]
        public void IndexShouldReturnIndexViewResult()
        {
            var controller = GetAdminController();

            controller.Index().Should().BeOfType<ViewResult>().Which.ViewName.Should().BeEquivalentTo("Index");
        }
    }
}