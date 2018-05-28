using AlphaDev.Web.Controllers;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Controllers
{
    public class AdminControllerTests
    {
        [NotNull]
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