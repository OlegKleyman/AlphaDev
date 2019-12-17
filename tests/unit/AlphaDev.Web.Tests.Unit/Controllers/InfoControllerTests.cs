using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
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
        private static InfoController GetInfoController(Option<IAboutService> aboutService = default,
            Option<IContactService> contactService = default)
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
            controller.About()
                      .Should()
                      .BeOfType<RedirectToActionResult>()
                      .Which.ActionName.Should()
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
        public async Task ContactShouldReturnContactView()
        {
            var contactService = Substitute.For<IContactService>();
            contactService.GetContactDetailsAsync().Returns(Option.Some<string>(default));
            var controller = GetInfoController(default, contactService.Some());
            (await controller.Contact())
                .Should()
                .BeOfType<ViewResult>()
                .Which.ViewName.Should()
                .BeEquivalentTo("Contact");
        }

        [Fact]
        public async Task ContactShouldReturnNoDetailsModelIfNoContactInformationExistsAndUserIsNotAuthenticated()
        {
            var controller = GetInfoController(default, Substitute.For<IContactService>().Some());
            (await controller.Contact()).Should().BeOfType<ViewResult>().Which.Model.Should().BeEquivalentTo("No details");
        }

        [Fact]
        public async Task ContactShouldReturnRedirectToCreateContactActionIfNoContactInformationExistsAndUserIsAuthenticated()
        {
            var controller = GetInfoController(Substitute.For<IAboutService>().Some());
            controller.User.Identity.IsAuthenticated.Returns(true);
            (await controller.Contact())
                      .Should()
                      .BeOfType<RedirectToActionResult>()
                      .Which.ActionName.Should()
                      .BeEquivalentTo("CreateContact");
        }

        [Fact]
        public async Task ContactShouldReturnStringModel()
        {
            var controller = GetInfoController(default, Substitute.For<IContactService>().Some());
            (await controller.Contact()).Should().BeOfType<ViewResult>().Which.Model.Should().BeOfType<string>();
        }

        [Fact]
        public async Task ContactShouldReturnStringModelFromFromContactService()
        {
            var contactService = Substitute.For<IContactService>();
            contactService.GetContactDetailsAsync().Returns(Option.Some("test"));
            var controller = GetInfoController(default, contactService.Some());
            (await controller.Contact()).Should().BeOfType<ViewResult>().Which.Model.Should().BeEquivalentTo("test");
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

            controller.CreateAbout()
                      .Should()
                      .BeOfType<RedirectToActionResult>()
                      .Which.ActionName.Should()
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
                      .Which.ViewName.Should()
                      .BeEquivalentTo("CreateAbout");
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
        public async Task CreateContactShouldCreateContactWhenModelIsValid()
        {
            var contactService = Substitute.For<IContactService>();
            var controller = GetInfoController(default, contactService.Some());
            const string value = "value";
            await controller.CreateContact(new ContactCreateViewModel(value));
            await contactService.Received(1).CreateAsync(value);
        }

        [Fact]
        public async Task CreateContactShouldRedirectToEditContactActionWhenThereIsExistingContact()
        {
            var contactService = Substitute.For<IContactService>();
            contactService.GetContactDetailsAsync().Returns(Option.Some("test"));
            var controller = GetInfoController(default, contactService.Some());

            (await controller.CreateContact())
                      .Should()
                      .BeOfType<RedirectToActionResult>()
                      .Which.ActionName.Should()
                      .BeEquivalentTo("EditContact");
        }

        [Fact]
        public async Task CreateContactShouldReturnCreateContactViewWhenModelIsNotValid()
        {
            var contactService = Substitute.For<IContactService>();
            var controller = GetInfoController(default, contactService.Some());
            controller.ModelState.AddModelError("test", "test");
            var result = await controller.CreateContact(new ContactCreateViewModel(default));
            result.Should().BeOfType<ViewResult>().Which.ViewName.Should().BeEquivalentTo("CreateContact");
        }

        [Fact]
        public async Task CreateContactShouldReturnCreateContactViewWhenThereIsNoExistingContact()
        {
            var contactService = Substitute.For<IContactService>();
            var controller = GetInfoController(default, contactService.Some());

            (await controller.CreateContact())
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.ViewName.Should()
                      .BeEquivalentTo("CreateContact");
        }

        [Fact]
        public async Task CreateContactShouldReturnRedirectToContactActionWhenModelIsValid()
        {
            var contactService = Substitute.For<IContactService>();
            var controller = GetInfoController(default, contactService.Some());
            var result = await controller.CreateContact(new ContactCreateViewModel(default));
            result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().BeEquivalentTo("Contact");
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
                      .Which.ViewName.Should()
                      .BeEquivalentTo("EditAbout");
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
        public async Task EditContactShouldEditContactWithModelData()
        {
            var contactService = Substitute.For<IContactService>();
            const string editValue = "value";

            var controller = GetInfoController(default, contactService.Some());

            var post = new ContactEditViewModel(editValue);
            await controller.EditContact(post);

            await contactService.Received(1).EditAsync(editValue);
        }

        [Fact]
        public async Task EditContactShouldReturnContactRedirectToActionResultWhenModelIsValid()
        {
            var contactService = Substitute.For<IContactService>();
            var controller = GetInfoController(default, contactService.Some());

            var post = new ContactEditViewModel(default);
            var result = await controller.EditContact(post);
            result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().BeEquivalentTo("Contact");
        }

        [Fact]
        public async Task EditContactShouldReturnContactViewResultWhenModelIsValid()
        {
            var contactService = Substitute.For<IContactService>();
            var controller = GetInfoController(default, contactService.Some());
            controller.ModelState.AddModelError(string.Empty, string.Empty);
            var post = new ContactEditViewModel(default);
            var result = await controller.EditContact(post);
            result.Should().BeOfType<ViewResult>().Which.ViewName.Should().BeEquivalentTo("EditContact");
        }

        [Fact]
        public async Task EditContactShouldReturnEditContactView()
        {
            var contactService = Substitute.For<IContactService>();
            contactService.GetContactDetailsAsync().Returns(Option.Some<string>(default));
            var controller = GetInfoController(default, contactService.Some());

            (await controller.EditContact())
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.ViewName.Should()
                      .BeEquivalentTo("EditContact");
        }

        [Fact]
        public async Task EditContactShouldReturnEditContactViewWithModelFromRepositoryWhenContactIsFound()
        {
            var contactService = Substitute.For<IContactService>();
            contactService.GetContactDetailsAsync().Returns(Option.Some("test"));

            var controller = GetInfoController(default, contactService.Some());

            (await controller.EditContact())
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
        public async Task EditContactShouldReturnRedirectActionResultToContactActionWhenContactIsNotFound()
        {
            var contactService = Substitute.For<IContactService>();

            var controller = GetInfoController(default, contactService.Some());

            (await controller.EditContact())
                      .Should()
                      .BeOfType<RedirectToActionResult>()
                      .Which.ActionName.Should()
                      .BeEquivalentTo("Contact");
        }
    }
}