using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Services.Web;
using AlphaDev.Services.Web.Models;
using AspNetCoreTestServer.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Optional;
using Refit;
using TechTalk.SpecFlow;

namespace AlphaDev.Web.Api.Tests.Integration.FeatureSteps
{
    [Binding]
    public class Security : Steps
    {
        [Given(@"I am an existing user")]
        public async Task GivenIAmAnExistingUser()
        {
            var user = new User
            {
                UserName = "something@something.com",
                AccessFailedCount = default,
                ConcurrencyStamp = "dda9cebb-c520-494c-b365-4823f1c0b938",
                Email = "something@something.com",
                EmailConfirmed = default,
                LockoutEnabled = true,
                Id = Guid.NewGuid().ToString("D"),
                LockoutEnd = default,
                NormalizedEmail = "SOMETHING@SOMETHING.COM",
                NormalizedUserName = "SOMETHING@SOMETHING.COM",
                PasswordHash = "AQAAAAEAACcQAAAAEC8J5oSDteONWmC4w/wTQj4ylVY+UnNjYWKgof2+VdMacVM2TbtW76DBx+Arx/1tzg==",
                PhoneNumber = default,
                PhoneNumberConfirmed = default,
                SecurityStamp = "8c0df882-fdeb-4e71-a588-77ee702facce",
                TwoFactorEnabled = default
            };

            var context = ScenarioContext.Get<IdentityDbContext<User>>();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            ScenarioContext.Set(new[] { user });
        }

        [When(@"I request a token")]
        public async Task WhenIRequestAToken()
        {
            var service = RestService.For<ITokenRestService>(ScenarioContext.Get<Uri>("BLOG_SERVICE_URL").AbsoluteUri);
            var user = ScenarioContext.Get<User[]>().First();

            var claims = new Claims(user.UserName, "H3ll04321!");
            ScenarioContext["AUTHENTICATION_TOKEN"] = await service.GetToken(claims);
        }

        [Then(@"I will receive a security token")]
        public void ThenIWillReceiveASecurityToken()
        {
            ScenarioContext.Get<string>("AUTHENTICATION_TOKEN").Should().NotBeEmpty();
        }
    }
}
