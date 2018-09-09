using AlphaDev.Core;
using AlphaDev.Web.Controllers;
using FluentAssertions;
using JetBrains.Annotations;
using NSubstitute;
using Optional;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Controllers
{
    public class InfoControllerTests
    {
        [NotNull]
        private static InfoController GetInfoController([NotNull] IInformationService informationService)
        {
            return new InfoController(informationService);
        }

        [Fact]
        public void AboutShouldReturnAboutView()
        {
            var controller = GetInfoController(Substitute.For<IInformationService>());
            controller.About().ViewName.Should().BeEquivalentTo("About");
        }

        [Fact]
        public void AboutShouldReturnSEmptytringModelIfNoAboutInformationExists()
        {
            var informationService = Substitute.For<IInformationService>();
            var controller = GetInfoController(informationService);
            controller.About().Model.Should().BeEquivalentTo(string.Empty);
        }

        [Fact]
        public void AboutShouldReturnStringModel()
        {
            var controller = GetInfoController(Substitute.For<IInformationService>());
            controller.About().Model.Should().BeOfType<string>();
        }

        [Fact]
        public void AboutShouldReturnStringModelFromFromInformationService()
        {
            var informationService = Substitute.For<IInformationService>();
            informationService.GetAboutDetails().Returns(Option.Some("test"));
            var controller = GetInfoController(informationService);
            controller.About().Model.Should().BeEquivalentTo("test");
        }
    }
}