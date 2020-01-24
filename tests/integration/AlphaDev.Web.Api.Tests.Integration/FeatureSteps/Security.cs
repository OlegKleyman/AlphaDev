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
        private readonly ScenarioContext _scenarioContext;
        private readonly IdentityDbContext<User> _context;
        private readonly IWebServer _server;

        public Security(ScenarioContext scenarioContext, IdentityDbContext<User> context, IWebServer server)
        {
            _scenarioContext = scenarioContext;
            _context = context;
            _server = server;
        }

        [BeforeScenario]
        public async Task StartServer()
        {
            var configuration = new Dictionary<string, string>
            {
                ["connectionStrings:security"] = _context.Database.GetDbConnection().ConnectionString,
                ["Jwt:Issuer"] = "issuer",
                ["Jwt:Audience"] = "audience",
                ["Jwt:Key"] = "key"
            };

            var state = await _server.StartAsync<Startup>(typeof(Startup).Assembly, Option.None<string>(),
                configuration);
            _scenarioContext["BLOG_SERVICE_URL"] = state.Endpoint;
            _scenarioContext["WEB_CONFIGURATION"] = configuration;
        }

        [BeforeScenario]
        public void InitializeDatabase()
        {
            _context.Database.Migrate();
        }

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

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            _scenarioContext.Set(new[] { user });
        }

        [When(@"I request a token")]
        public async Task WhenIRequestAToken()
        {
            var service = RestService.For<ITokenRestService>(_scenarioContext.Get<Uri>("BLOG_SERVICE_URL").AbsoluteUri);
            var user = _scenarioContext.Get<User[]>().First();

            var claims = new Claims(user.UserName, "H3ll04321!");
            _scenarioContext["AUTHENTICATION_TOKEN"] = await service.GetToken(claims);
        }

        [Then(@"I will receive a security token")]
        public void ThenIWillReceiveASecurityToken()
        {
            _scenarioContext.Get<string>("AUTHENTICATION_TOKEN").Should().NotBeEmpty();
        }
    }
}
