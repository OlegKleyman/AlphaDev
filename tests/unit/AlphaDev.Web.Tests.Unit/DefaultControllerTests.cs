namespace AlphaDev.Web.Tests.Unit
{
    using AlphaDev.Web.Controllers;

    using FluentAssertions.Mvc;

    using Xunit;

    public class DefaultControllerTests
    {
        private DefaultController GetDefaultController() => new DefaultController();

        [Fact]
        public void IndexShouldReturnIndexView()
        {
            var controller = GetDefaultController();

            controller.Index().Should().BeViewResult();
        }
    }
}