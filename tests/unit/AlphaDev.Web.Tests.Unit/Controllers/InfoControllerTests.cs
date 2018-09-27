using System.Security.Claims;
using System.Security.Principal;
using AlphaDev.Core;
using AlphaDev.Web.Controllers;
using AlphaDev.Web.Models;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Optional;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Controllers
{
    public class InfoControllerTests
    {
        [NotNull]
        private static InfoController GetInfoController([NotNull] IAboutService aboutService)
        {
            var identity = Substitute.For<IIdentity>();
            var user = Substitute.For<ClaimsPrincipal>();
            user.Identity.Returns(identity);
            var controller = new InfoController(aboutService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            return controller;
        }

        [Fact]
        public void AboutShouldReturnAboutView()
        {
            var informationService = Substitute.For<IAboutService>();
            informationService.GetAboutDetails().Returns(Option.Some<string>(default));
            var controller = GetInfoController(informationService);
            controller.About().Should().BeOfType<ViewResult>().Which.ViewName.Should().BeEquivalentTo("About");
        }

        [Fact]
        public void AboutShouldReturnNoDetailsModelIfNoAboutInformationExistsAndUserIsNotAuthenticated()
        {
            var informationService = Substitute.For<IAboutService>();
            var controller = GetInfoController(informationService);
            controller.About().Should().BeOfType<ViewResult>().Which.Model.Should().BeEquivalentTo("No details");
        }

        [Fact]
        public void AboutShouldReturnRedirectToCreateAboutActionIfNoAboutInformationExistsAndUserIsAuthenticated()
        {
            var informationService = Substitute.For<IAboutService>();
            var controller = GetInfoController(informationService);
            controller.User.Identity.IsAuthenticated.Returns(true);
            controller.About().Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should()
                .BeEquivalentTo("CreateAbout");
        }

        [Fact]
        public void AboutShouldReturnStringModel()
        {
            var controller = GetInfoController(Substitute.For<IAboutService>());
            controller.About().Should().BeOfType<ViewResult>().Which.Model.Should().BeOfType<string>();
        }

        [Fact]
        public void AboutShouldReturnStringModelFromFromInformationService()
        {
            var informationService = Substitute.For<IAboutService>();
            informationService.GetAboutDetails().Returns(Option.Some("test"));
            var controller = GetInfoController(informationService);
            controller.About().Should().BeOfType<ViewResult>().Which.Model.Should().BeEquivalentTo("test");
        }

        [Fact]
        public void CreateAboutShouldCreateAboutWhenModelIsValid()
        {
            var informationService = Substitute.For<IAboutService>();
            var controller = GetInfoController(informationService);
            const string value = "value";
            controller.CreateAbout(new AboutCreateViewModel(value));
            informationService.Received(1).Create(value);
        }

        [Fact]
        public void CreateAboutShouldRedirectToEditAboutActionWhenThereIsExistingAbout()
        {
            var informationService = Substitute.For<IAboutService>();
            informationService.GetAboutDetails().Returns(Option.Some("test"));
            var controller = GetInfoController(informationService);

            controller.CreateAbout().Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should()
                .BeEquivalentTo("EditAbout");
        }

        [Fact]
        public void CreateAboutShouldReturnCreateAboutViewWhenModelIsNotValid()
        {
            var informationService = Substitute.For<IAboutService>();
            var controller = GetInfoController(informationService);
            controller.ModelState.AddModelError("test", "test");
            var result = controller.CreateAbout(new AboutCreateViewModel(default));
            result.Should().BeOfType<ViewResult>().Which.ViewName.Should().BeEquivalentTo("CreateAbout");
        }

        [Fact]
        public void CreateAboutShouldReturnCreateAboutViewWhenThereIsNoExistingAbout()
        {
            var informationService = Substitute.For<IAboutService>();
            var controller = GetInfoController(informationService);

            controller.CreateAbout()
                .Should()
                .BeOfType<ViewResult>()
                .Which.ViewName.Should().BeEquivalentTo("CreateAbout");
        }

        [Fact]
        public void CreateAboutShouldReturnRedirectToAboutActionWhenModelIsValid()
        {
            var informationService = Substitute.For<IAboutService>();
            var controller = GetInfoController(informationService);
            var result = controller.CreateAbout(new AboutCreateViewModel(default));
            result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().BeEquivalentTo("About");
        }

        [Fact]
        public void EditAboutShouldEditAboutWithModelData()
        {
            var informationService = Substitute.For<IAboutService>();
            const string editValue = "value";

            var controller = GetInfoController(informationService);

            var post = new AboutEditViewModel(editValue);
            controller.EditAbout(post);

            informationService.Received(1).Edit(editValue);
        }

        [Fact]
        public void EditAboutShouldReturnAboutRedirectToActionResultWhenModelIsValid()
        {
            var informationService = Substitute.For<IAboutService>();
            var controller = GetInfoController(informationService);

            var post = new AboutEditViewModel(default);
            var result = controller.EditAbout(post);
            result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().BeEquivalentTo("About");
        }

        [Fact]
        public void EditAboutShouldReturnAboutViewResultWhenModelIsValid()
        {
            var informationService = Substitute.For<IAboutService>();
            var controller = GetInfoController(informationService);
            controller.ModelState.AddModelError(string.Empty, string.Empty);
            var post = new AboutEditViewModel(default);
            var result = controller.EditAbout(post);
            result.Should().BeOfType<ViewResult>().Which.ViewName.Should().BeEquivalentTo("EditAbout");
        }

        [Fact]
        public void EditAboutShouldReturnEditAboutView()
        {
            var informationService = Substitute.For<IAboutService>();
            informationService.GetAboutDetails().Returns(Option.Some<string>(default));
            var controller = GetInfoController(informationService);

            controller.EditAbout()
                .Should()
                .BeOfType<ViewResult>()
                .Which.ViewName.Should().BeEquivalentTo("EditAbout");
        }

        [Fact]
        public void EditAboutShouldReturnEditAboutViewWithModelFromRepositoryWhenAboutIsFound()
        {
            var informationService = Substitute.For<IAboutService>();
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
        public void EditAboutShouldReturnRedirectActionResultToAboutActionWhenAboutIsNotFound()
        {
            var informationService = Substitute.For<IAboutService>();

            var controller = GetInfoController(informationService);

            controller
                .EditAbout()
                .Should()
                .BeOfType<RedirectToActionResult>()
                .Which.ActionName.Should()
                .BeEquivalentTo("About");
        }
    }
}