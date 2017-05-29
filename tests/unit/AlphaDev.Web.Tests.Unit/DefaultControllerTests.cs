namespace AlphaDev.Web.Tests.Unit
{
    using AlphaDev.Web.Controllers;

    using FluentAssertions.Mvc;

    using Xunit;

    public class DefaultControllerTests
    {
        [Fact]
        public void IndexShouldReturnIndexView()
        {
            var controller = GetDefaultController();

            controller.Index().Should().BeViewResult();
        }

        private DefaultController GetDefaultController() => new DefaultController();
    }
}
