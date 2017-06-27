namespace AlphaDev.Web.Tests.Unit
{
    using AlphaDev.Web.Controllers;

    using AppDev.Core;

    using FluentAssertions;
    using FluentAssertions.Mvc;

    using NSubstitute;

    using Xunit;

    public class DefaultControllerTests
    {
        private DefaultController GetDefaultController()
        {
            var blogService = Substitute.For<IBlogService>();
            blogService.GetLatest().Returns(new Blog(null, null, null));

            return new DefaultController(blogService);
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

            controller.Index().Model.Should().BeOfType<Blog>();
        }
    }
}