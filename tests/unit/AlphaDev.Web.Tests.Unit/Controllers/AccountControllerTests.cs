﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Web.Controllers;
using AlphaDev.Web.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Optional;
using Optional.Collections;
using Optional.Unsafe;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AlphaDev.Web.Tests.Unit.Controllers
{
    public class AccountControllerTests
    {
        [Fact]
        public void LoginShouldReturnLoginView()
        {
            var controller = GetAccountController();

            controller.Login().Should().BeOfType<ViewResult>().Which.ViewName.Should().BeEquivalentTo("Login");
        }

        [Fact]
        public void LoginShouldSetViewDataWhenReturnUrlIsUsed()
        {
            var controller = GetAccountController();

            controller.Login("test").Should().BeOfType<ViewResult>().Which.ViewData["ReturnUrl"].Should().BeEquivalentTo("test");
        }

        [Fact]
        public void LoginShouldReturnEmptyLoginViewModel()
        {
            var controller = GetAccountController();

            controller.Login().Should().BeOfType<ViewResult>().Which.Model.Should().BeOfType<LoginViewModel>().Which
                .Should().BeEquivalentTo(new LoginViewModel());
        }

        [Fact]
        public void LoginShouldReturnLoginViewWhenLoginIsNotSuccessful()
        {
            var controller = GetAccountController();

            var model = new LoginViewModel
            {
                Username = "test",
                Password = "pass"
            };

            controller.Login(model).Should().BeOfType<ViewResult>().Which.ViewName.Should()
                .BeEquivalentTo("Login");
        }

        [Fact]
        public void LoginShouldReturnLoginViewModelWhenLoginIsNotSuccessful()
        {
            var controller = GetAccountController();

            var model = new LoginViewModel
            {
                Username = "test",
                Password = "pass"
            };

            controller.Login(model).Should().BeOfType<ViewResult>().Which.Model.Should()
                .BeEquivalentTo(model);
        }

        [Fact]
        public void LoginShouldReturnRedirectResultWhenTheLoginIsSuccessfull()
        {
            var controller = GetAccountController();

            var model = new LoginViewModel
            {
                Username = "test",
                Password = "working"
            };

            controller.Login(model).Should()
                .BeOfType<RedirectResult>();
        }

        [Fact]
        public void LoginShouldReturnRedirectToRootWhenNoRedirectUrlIsPassedResultWhenTheLoginIsSuccessfull()
        {
            var controller = GetAccountController();

            var model = new LoginViewModel
            {
                Username = "test",
                Password = "working"
            };

            controller.Login(model).Should()
                .BeOfType<RedirectResult>().Which.Url.Should().BeEquivalentTo("/");
        }

        [Fact]
        public void LoginShouldReturnRedirectToUrlThatIsPassedResultWhenTheLoginIsSuccessfull()
        {
            var controller = GetAccountController();

            var model = new LoginViewModel
            {
                Username = "test",
                Password = "working"
            };

            controller.Login(model, "admin").Should()
                .BeOfType<RedirectResult>().Which.Url.Should().BeEquivalentTo("admin");
        }

        [Fact]
        public void LoginShouldReturnLoginViewWhenModelStateIsInvalid()
        {
            var controller = GetAccountController();

            controller.ModelState.AddModelError("fail", "fail");

            var model = new LoginViewModel
            {
                Username = "test",
                Password = "working"
            };

            controller.Login(model).Should().BeOfType<ViewResult>().Which.ViewName.Should().BeEquivalentTo("Login");
        }

        [Fact]
        public void LoginShouldReturnLoginErrorOnInvalidLogin()
        {
            var controller = GetAccountController();

            controller.ModelState.AddModelError("fail", "fail");

            var model = new LoginViewModel
            {
                Username = "test",
                Password = "pass"
            };

            controller.Login(model);

            controller.ModelState.Should().Contain(pair => pair.Key == string.Empty).Which.Value?.Errors?.Should()
                .Contain(error => error.ErrorMessage == "Invalid login");
        }

        private AccountController GetAccountController()
        {
            var userManager = Substitute.For<UserManager<User>>(
                Substitute.For<IUserStore<User>, IUserPasswordStore<User>>(),
                Substitute.For<IOptions<IdentityOptions>>(),
                Substitute.For<IPasswordHasher<User>>(), null, null, null, null, null,
                Substitute.For<ILogger<UserManager<User>>>());

            var signInManager = Substitute.For<SignInManager<User>>(
                userManager,
                new HttpContextAccessor(), Substitute.For<IUserClaimsPrincipalFactory<User>>(), null,
                Substitute.For<ILogger<SignInManager<User>>>(), null);

            signInManager.PasswordSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
                .Returns(Task.FromResult(SignInResult.Failed));

            signInManager.PasswordSignInAsync(Arg.Any<string>(), "working", Arg.Any<bool>(), Arg.Any<bool>())
                .Returns(Task.FromResult(SignInResult.Success));
            
            return new AccountController(signInManager);
        }
    }
}