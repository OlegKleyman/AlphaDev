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
        private static InfoController GetInfoController(Option<IAboutService> aboutService = default, Option<IContactService> contactService = default)
        {
            var identity = Substitute.For<IIdentity>();
            var user = Substitute.For<ClaimsPrincipal>();
            user.Identity.Returns(identity);
            var controller = new InfoController(aboutService.ValueOr(Substitute.For<IAboutService>()),
                contactService.ValueOr(Substitute.For<IContactService>()))
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } }
            };

            return controller;
        }

        [Fact]
        public void AboutShouldReturnAboutView()
        {
            var informationService = Substitute.For<IAboutService>();
            informationService.GetAboutDetails().Returns(Option.Some<string>(default));
            var controller = GetInfoController(informationService.Some());
            controller.About().Should().BeOfType<ViewResult>().Which.ViewName.Should().BeEquivalentTo("About");
        }

        [Fact]
        public void AboutShouldReturnNoDetailsModelIfNoAboutInformationExistsAndUserIsNotAuthenticated()
        {
            var informationService = Substitute.For<IAboutService>();
            var controller = GetInfoController(informationService.Some());
            controller.About().Should().BeOfType<ViewResult>().Which.Model.Should().BeEquivalentTo("No details");
        }

        [Fact]
        public void AboutShouldReturnRedirectToCreateAboutActionIfNoAboutInformationExistsAndUserIsAuthenticated()
        {
            var informationService = Substitute.For<IAboutService>();
            var controller = GetInfoController(informationService.Some());
            controller.User.Identity.IsAuthenticated.Returns(true);
            controller.About().Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should()
                .BeEquivalentTo("CreateAbout");
        }

        [Fact]
        public void AboutShouldReturnStringModel()
        {
            var controller = GetInfoController(Substitute.For<IAboutService>().Some());
            controller.About().Should().BeOfType<ViewResult>().Which.Model.Should().BeOfType<string>();
        }

        [Fact]
        public void AboutShouldReturnStringModelFromFromInformationService()
        {
            var informationService = Substitute.For<IAboutService>();
            informationService.GetAboutDetails().Returns(Option.Some("test"));
            var controller = GetInfoController(informationService.Some());
            controller.About().Should().BeOfType<ViewResult>().Which.Model.Should().BeEquivalentTo("test");
        }

        [Fact]
        public void CreateAboutShouldCreateAboutWhenModelIsValid()
        {
            var informationService = Substitute.For<IAboutService>();
            var controller = GetInfoController(informationService.Some());
            const string value = "value";
            controller.CreateAbout(new AboutCreateViewModel(value));
            informationService.Received(1).Create(value);
        }

        [Fact]
        public void CreateAboutShouldRedirectToEditAboutActionWhenThereIsExistingAbout()
        {
            var informationService = Substitute.For<IAboutService>();
            informationService.GetAboutDetails().Returns(Option.Some("test"));
            var controller = GetInfoController(informationService.Some());

            controller.CreateAbout().Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should()
                .BeEquivalentTo("EditAbout");
        }

        [Fact]
        public void CreateAboutShouldReturnCreateAboutViewWhenModelIsNotValid()
        {
            var informationService = Substitute.For<IAboutService>();
            var controller = GetInfoController(informationService.Some());
            controller.ModelState.AddModelError("test", "test");
            var result = controller.CreateAbout(new AboutCreateViewModel(default));
            result.Should().BeOfType<ViewResult>().Which.ViewName.Should().BeEquivalentTo("CreateAbout");
        }

        [Fact]
        public void CreateAboutShouldReturnCreateAboutViewWhenThereIsNoExistingAbout()
        {
            var informationService = Substitute.For<IAboutService>();
            var controller = GetInfoController(informationService.Some());

            controller.CreateAbout()
                .Should()
                .BeOfType<ViewResult>()
                .Which.ViewName.Should().BeEquivalentTo("CreateAbout");
        }

        [Fact]
        public void CreateAboutShouldReturnRedirectToAboutActionWhenModelIsValid()
        {
            var informationService = Substitute.For<IAboutService>();
            var controller = GetInfoController(informationService.Some());
            var result = controller.CreateAbout(new AboutCreateViewModel(default));
            result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().BeEquivalentTo("About");
        }

        [Fact]
        public void EditAboutShouldEditAboutWithModelData()
        {
            var informationService = Substitute.For<IAboutService>();
            const string editValue = "value";

            var controller = GetInfoController(informationService.Some());

            var post = new AboutEditViewModel(editValue);
            controller.EditAbout(post);

            informationService.Received(1).Edit(editValue);
        }

        [Fact]
        public void EditAboutShouldReturnAboutRedirectToActionResultWhenModelIsValid()
        {
            var informationService = Substitute.For<IAboutService>();
            var controller = GetInfoController(informationService.Some());

            var post = new AboutEditViewModel(default);
            var result = controller.EditAbout(post);
            result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().BeEquivalentTo("About");
        }

        [Fact]
        public void EditAboutShouldReturnAboutViewResultWhenModelIsValid()
        {
            var informationService = Substitute.For<IAboutService>();
            var controller = GetInfoController(informationService.Some());
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
            var controller = GetInfoController(informationService.Some());

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

            var controller = GetInfoController(informationService.Some());

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

            var controller = GetInfoController(informationService.Some());

            controller
                .EditAbout()
                .Should()
                .BeOfType<RedirectToActionResult>()
                .Which.ActionName.Should()
                .BeEquivalentTo("About");
        }

        [Fact]
        public void ContactShouldReturnContactView()
        {
            var contactService = Substitute.For<IContactService>();
            contactService.GetDetails().Returns(Option.Some<string>(default));
            var controller = GetInfoController(default, contactService.Some());
            controller.Contact().Should().BeOfType<ViewResult>().Which.ViewName.Should().BeEquivalentTo("Contact");
        }

        [Fact]
        public void ContactShouldReturnNoDetailsModelIfNoContactInformationExistsAndUserIsNotAuthenticated()
        {
            var controller = GetInfoController(default, Substitute.For<IContactService>().Some());
            controller.Contact().Should().BeOfType<ViewResult>().Which.Model.Should().BeEquivalentTo("No details");
        }

        [Fact]
        public void ContactShouldReturnRedirectToCreateContactActionIfNoContactInformationExistsAndUserIsAuthenticated()
        {
            var controller = GetInfoController(Substitute.For<IAboutService>().Some());
            controller.User.Identity.IsAuthenticated.Returns(true);
            controller.Contact().Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should()
                .BeEquivalentTo("CreateContact");
        }

        [Fact]
        public void ContactShouldReturnStringModel()
        {
            var controller = GetInfoController(default, Substitute.For<IContactService>().Some());
            controller.Contact().Should().BeOfType<ViewResult>().Which.Model.Should().BeOfType<string>();
        }

        [Fact]
        public void ContactShouldReturnStringModelFromFromContactService()
        {
            var contactService = Substitute.For<IContactService>();
            contactService.GetDetails().Returns(Option.Some("test"));
            var controller = GetInfoController(default, contactService.Some());
            controller.Contact().Should().BeOfType<ViewResult>().Which.Model.Should().BeEquivalentTo("test");
        }

        [Fact]
        public void EditContactShouldEditContactWithModelData()
        {
            var contactService = Substitute.For<IContactService>();
            const string editValue = "value";

            var controller = GetInfoController(default, contactService.Some());

            var post = new ContactEditViewModel(editValue);
            controller.EditContact(post);

            contactService.Received(1).Edit(editValue);
        }

        [Fact]
        public void EditContactShouldReturnContactRedirectToActionResultWhenModelIsValid()
        {
            var contactService = Substitute.For<IContactService>();
            var controller = GetInfoController(default, contactService.Some());

            var post = new ContactEditViewModel(default);
            var result = controller.EditContact(post);
            result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().BeEquivalentTo("Contact");
        }

        [Fact]
        public void EditContactShouldReturnContactViewResultWhenModelIsValid()
        {
            var contactService = Substitute.For<IContactService>();
            var controller = GetInfoController(default, contactService.Some());
            controller.ModelState.AddModelError(string.Empty, string.Empty);
            var post = new ContactEditViewModel(default);
            var result = controller.EditContact(post);
            result.Should().BeOfType<ViewResult>().Which.ViewName.Should().BeEquivalentTo("EditContact");
        }

        [Fact]
        public void EditContactShouldReturnEditContactView()
        {
            var contactService = Substitute.For<IContactService>();
            contactService.GetDetails().Returns(Option.Some<string>(default));
            var controller = GetInfoController(default, contactService.Some());

            controller.EditContact()
                .Should()
                .BeOfType<ViewResult>()
                .Which.ViewName.Should().BeEquivalentTo("EditContact");
        }

        [Fact]
        public void EditContactShouldReturnEditContactViewWithModelFromRepositoryWhenContactIsFound()
        {
            var contactService = Substitute.For<IContactService>();
            contactService.GetDetails().Returns(Option.Some("test"));

            var controller = GetInfoController(default, contactService.Some());

            controller
                .EditContact()
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
        public void EditContactShouldReturnRedirectActionResultToContactActionWhenContactIsNotFound()
        {
            var contactService = Substitute.For<IContactService>();

            var controller = GetInfoController(default, contactService.Some());

            controller
                .EditContact()
                .Should()
                .BeOfType<RedirectToActionResult>()
                .Which.ActionName.Should()
                .BeEquivalentTo("Contact");
        }

        [Fact]
        public void CreateContactShouldCreateContactWhenModelIsValid()
        {
            var contactService = Substitute.For<IContactService>();
            var controller = GetInfoController(default, contactService.Some());
            const string value = "value";
            controller.CreateContact(new ContactCreateViewModel(value));
            contactService.Received(1).Create(value);
        }

        [Fact]
        public void CreateContactShouldRedirectToEditContactActionWhenThereIsExistingContact()
        {
            var contactService = Substitute.For<IContactService>();
            contactService.GetDetails().Returns(Option.Some("test"));
            var controller = GetInfoController(default, contactService.Some());

            controller.CreateContact().Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should()
                .BeEquivalentTo("EditContact");
        }

        [Fact]
        public void CreateContactShouldReturnCreateContactViewWhenModelIsNotValid()
        {
            var contactService = Substitute.For<IContactService>();
            var controller = GetInfoController(default, contactService.Some());
            controller.ModelState.AddModelError("test", "test");
            var result = controller.CreateContact(new ContactCreateViewModel(default));
            result.Should().BeOfType<ViewResult>().Which.ViewName.Should().BeEquivalentTo("CreateContact");
        }

        [Fact]
        public void CreateContactShouldReturnCreateContactViewWhenThereIsNoExistingContact()
        {
            var contactService = Substitute.For<IContactService>();
            var controller = GetInfoController(default, contactService.Some());

            controller.CreateContact()
                .Should()
                .BeOfType<ViewResult>()
                .Which.ViewName.Should().BeEquivalentTo("CreateContact");
        }

        [Fact]
        public void CreateContactShouldReturnRedirectToContactActionWhenModelIsValid()
        {
            var contactService = Substitute.For<IContactService>();
            var controller = GetInfoController(default, contactService.Some());
            var result = controller.CreateContact(new ContactCreateViewModel(default));
            result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().BeEquivalentTo("Contact");
        }
    }
}