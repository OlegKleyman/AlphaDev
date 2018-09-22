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
        public void AboutShouldReturnSNoDetailsModelIfNoAboutInformationExists()
        {
            var informationService = Substitute.For<IInformationService>();
            var controller = GetInfoController(informationService);
            controller.About().Model.Should().BeEquivalentTo("No details");
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

        [Fact]
        public void EditAboutShouldReturnEditAboutView()
        {
            var informationService = Substitute.For<IInformationService>();

            var controller = GetInfoController(informationService);

            controller.EditAbout()
                .Should()
                .BeOfType<ViewResult>()
                .Which.ViewName.Should().BeEquivalentTo("EditAbout");
        }

        [Fact]
        public void EditAboutShouldReturnEditAboutViewWithModelFromRepositoryWhenAboutIsFound()
        {
            var informationService = Substitute.For<IInformationService>();
            informationService.GetAboutDetails().Returns(Option.Some("test"));

            var controller = GetInfoController(informationService);

            controller
                .EditAbout()
                .Should()
                .BeOfType<ViewResult>()
                .Which.Model.Should()
                .BeEquivalentTo(
                    new
                    {
                        Value = "test"
                    });
        }

        [Fact]
        public void EditAboutShouldReturnEditAboutViewWithModelWithEmptyStringWhenAboutIsNotFound()
        {
            var informationService = Substitute.For<IInformationService>();

            var controller = GetInfoController(informationService);

            controller
                .EditAbout()
                .Should()
                .BeOfType<ViewResult>()
                .Which.Model.Should()
                .BeEquivalentTo(
                    new
                    {
                        Value = string.Empty
                    });
        }

        [Fact]
        public void EditAboutShouldEditAboutWithModelData()
        {
            var informationService = Substitute.For<IInformationService>();
            const string editValue = "value";
            
            var controller = GetInfoController(informationService);

            var post = new AboutEditorViewModel(editValue);
            controller.EditAbout(post);

            informationService.Received(1).Edit(editValue);
        }

        [Fact]
        public void EditAboutShouldReturnAboutRedirectToActionResultWhenModelIsValid()
        {
            var informationService = Substitute.For<IInformationService>();
            var controller = GetInfoController(informationService);

            var post = new AboutEditorViewModel(default);
            var result = controller.EditAbout(post);
            result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().BeEquivalentTo("About");
        }

        [Fact]
        public void EditAboutShouldReturnAboutViewResultWhenModelIsValid()
        {
            var informationService = Substitute.For<IInformationService>();
            var controller = GetInfoController(informationService);
            controller.ModelState.AddModelError(string.Empty, string.Empty);
            var post = new AboutEditorViewModel(default);
            var result = controller.EditAbout(post);
            result.Should().BeOfType<ViewResult>().Which.ViewName.Should().BeEquivalentTo("EditAbout");
        }
    }
}