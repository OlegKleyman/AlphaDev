using System.Threading.Tasks;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Web.Controllers;
using AlphaDev.Web.Models;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AlphaDev.Web.Tests.Unit.Controllers
{
    public class AccountControllerTests
    {
        [NotNull]
        private AccountController GetAccountController() => GetAccountController(GetMockSignInManager());

        [NotNull]
        private AccountController GetAccountController([NotNull] SignInManager<User> signInManager) =>
            new AccountController(signInManager);

        [NotNull]
        private static SignInManager<User> GetMockSignInManager()
        {
            var userManager = Substitute.For<UserManager<User>>(
                Substitute.For<IUserStore<User>, IUserPasswordStore<User>>(),
                Substitute.For<IOptions<IdentityOptions>>(),
                Substitute.For<IPasswordHasher<User>>(), null, null, null, null, null,
                Substitute.For<ILogger<UserManager<User>>>());

            var signInManager = Substitute.For<SignInManager<User>>(
                userManager,
                new HttpContextAccessor(), Substitute.For<IUserClaimsPrincipalFactory<User>>(), null,
                Substitute.For<ILogger<SignInManager<User>>>(), null, null);

            signInManager.PasswordSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
                         .Returns(Task.FromResult(SignInResult.Failed));

            signInManager.PasswordSignInAsync(Arg.Any<string>(), "working", Arg.Any<bool>(), Arg.Any<bool>())
                         .Returns(Task.FromResult(SignInResult.Success));

            return signInManager;
        }

        [Fact]
        public void LoginShouldReturnEmptyLoginViewModel()
        {
            var controller = GetAccountController();

            controller.Login()
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.Model.Should()
                      .BeOfType<LoginViewModel>()
                      .Which
                      .Should()
                      .BeEquivalentTo(new LoginViewModel());
        }

        [Fact]
        public async Task LoginShouldReturnLoginErrorOnInvalidLogin()
        {
            var controller = GetAccountController();

            var model = new LoginViewModel
            {
                Username = "test",
                Password = "pass"
            };

            await controller.Login(model);

            controller.ModelState.Should()
                      .Contain(pair => pair.Key == string.Empty)
                      .Which.Value?.Errors?.Should()
                      .Contain(error => error.ErrorMessage == "Invalid login");
        }

        [Fact]
        public void LoginShouldReturnLoginView()
        {
            var controller = GetAccountController();

            controller.Login().Should().BeOfType<ViewResult>().Which.ViewName.Should().BeEquivalentTo("Login");
        }

        [Fact]
        public async Task LoginShouldReturnLoginViewModelWhenLoginIsNotSuccessful()
        {
            var controller = GetAccountController();

            var model = new LoginViewModel
            {
                Username = "test",
                Password = "pass"
            };

            (await controller.Login(model))
                .Should()
                .BeOfType<ViewResult>()
                .Which.Model.Should()
                .BeEquivalentTo(model);
        }

        [Fact]
        public async Task LoginShouldReturnLoginViewModelWhenModelIsInvalid()
        {
            var controller = GetAccountController();
            controller.ModelState.AddModelError("test", "error");

            var model = new LoginViewModel
            {
                Username = "test",
                Password = "working"
            };

            (await controller.Login(model))
                .Should()
                .BeOfType<ViewResult>()
                .Which.Model.Should()
                .BeEquivalentTo(model);
        }

        [Fact]
        public async Task LoginShouldReturnLoginViewWhenLoginIsNotSuccessful()
        {
            var controller = GetAccountController();

            var model = new LoginViewModel
            {
                Username = "test",
                Password = "pass"
            };

            (await controller.Login(model))
                .Should()
                .BeOfType<ViewResult>()
                .Which.ViewName.Should()
                .BeEquivalentTo("Login");
        }

        [Fact]
        public async Task LoginShouldReturnLoginViewWhenModelIsInvalid()
        {
            var controller = GetAccountController();
            controller.ModelState.AddModelError("test", "error");

            (await controller.Login(new LoginViewModel()))
                .Should()
                .BeOfType<ViewResult>()
                .Which.ViewName.Should()
                .BeEquivalentTo("Login");
        }

        [Fact]
        public async Task LoginShouldReturnLoginViewWhenModelStateIsInvalid()
        {
            var controller = GetAccountController();

            controller.ModelState.AddModelError("fail", "fail");

            var model = new LoginViewModel
            {
                Username = "test",
                Password = "working"
            };

            (await controller.Login(model))
                .Should()
                .BeOfType<ViewResult>()
                .Which.ViewName.Should()
                .BeEquivalentTo("Login");
        }

        [Fact]
        public async Task LoginShouldReturnRedirectResultWhenTheLoginIsSuccessfull()
        {
            var controller = GetAccountController();

            var model = new LoginViewModel
            {
                Username = "test",
                Password = "working"
            };

            (await controller.Login(model))
                .Should()
                .BeOfType<RedirectResult>();
        }

        [Fact]
        public async Task LoginShouldReturnRedirectToRootWhenNoRedirectUrlIsPassedResultWhenTheLoginIsSuccessfull()
        {
            var controller = GetAccountController();

            var model = new LoginViewModel
            {
                Username = "test",
                Password = "working"
            };

            (await controller.Login(model))
                .Should()
                .BeOfType<RedirectResult>()
                .Which.Url.Should()
                .BeEquivalentTo("/");
        }

        [Fact]
        public async Task LoginShouldReturnRedirectToUrlThatIsPassedResultWhenTheLoginIsSuccessfull()
        {
            var controller = GetAccountController();

            var model = new LoginViewModel
            {
                Username = "test",
                Password = "working"
            };

            (await controller.Login(model, "admin"))
                .Should()
                .BeOfType<RedirectResult>()
                .Which.Url.Should()
                .BeEquivalentTo("admin");
        }

        [Fact]
        public void LoginShouldSetViewDataWhenReturnUrlIsUsed()
        {
            var controller = GetAccountController();

            controller.Login("test")
                      .Should()
                      .BeOfType<ViewResult>()
                      .Which.ViewData["ReturnUrl"]
                      .Should()
                      .BeEquivalentTo("test");
        }

        [Fact]
        public async void LogoutShouldLogoutUser()
        {
            var signInManager = GetMockSignInManager();

            var controller = GetAccountController(signInManager);

            await controller.Logout();

            await signInManager.Received().SignOutAsync();
        }

        [Fact]
        public async void LogoutShouldRedirectToIndexActionInDefaultController()
        {
            var controller = GetAccountController();

            var result = (await controller.Logout()).Should().BeOfType<RedirectToActionResult>().Which;

            result.ActionName.Should().BeEquivalentTo("Index");
            result.ControllerName.Should().BeEquivalentTo("Default");
        }
    }
}