using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Security;
using AlphaDev.Web.Api.Controllers;
using AlphaDev.Web.Api.Models;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace AlphaDev.Web.Api.Tests.Unit.Controllers
{
    public class TokenControllerTests
    {
        [Fact]
        public async Task GetTokenReturnsOkResultWhenValidRequestAndSuccessfulLogin()
        {
            var userManager = GetUserManagerMock();
            var securityTokenWriter = Substitute.For<ISecurityTokenWriter>();

            var user = new User();
            userManager.FindByNameAsync(nameof(Claims.Username)).Returns(user);
            userManager.CheckPasswordAsync(user, nameof(Claims.Password)).Returns(true);

            securityTokenWriter.Generate(Arg.Is<Claim[]>(c =>
                                   c.Length == 1 && c.First().Type == JwtRegisteredClaimNames.Sub &&
                                   c.First().Value == nameof(Claims.Username)))
                               .Returns("hash");

            var controller = GetTokenController(userManager, securityTokenWriter);
            var claims = new Claims
            {
                Password = nameof(Claims.Password),
                Username = nameof(Claims.Username)
            };

            var result = await controller.GetToken(claims);
            result.Should()
                  .BeOfType<OkObjectResult>()
                  .Which.Value.Should()
                  .BeOfType<string>()
                  .Which.Should()
                  .Be("hash");
        }

        [Fact]
        public async Task GetTokenReturnsUnauthorizedResultWhenLoginPasswordIsInvalid()
        {
            var userManager = GetUserManagerMock();

            var user = new User();
            userManager.FindByNameAsync(nameof(Claims.Username)).Returns(user);
            userManager.CheckPasswordAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(true);
            userManager.CheckPasswordAsync(user, nameof(Claims.Password)).Returns(false);

            var controller = GetTokenController(userManager, Substitute.For<ISecurityTokenWriter>());
            var claims = new Claims
            {
                Password = nameof(Claims.Password),
                Username = nameof(Claims.Username)
            };

            var result = await controller.GetToken(claims);
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task GetTokenReturnsUnauthorizedResultWhenLoginUsernameIsInvalid()
        {
            var userManager = GetUserManagerMock();

            userManager.FindByNameAsync(Arg.Any<string>()).Returns(new User());
            userManager.FindByNameAsync(nameof(Claims.Username)).Returns(default(User)!); // Using "!" because the method does indeed return null when user is not found
            userManager.CheckPasswordAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(true);
            userManager.CheckPasswordAsync(null!, nameof(Claims.Password)).Returns(false); // Using "!" because the User can indeed be null

            var controller = GetTokenController(userManager, Substitute.For<ISecurityTokenWriter>());
            var claims = new Claims
            {
                Password = nameof(Claims.Password),
                Username = nameof(Claims.Username)
            };

            var result = await controller.GetToken(claims);
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task GetTokenReturnsBadRequestResultWhenModelStateIsInvalid()
        {
            var controller = GetTokenController(GetUserManagerMock(), Substitute.For<ISecurityTokenWriter>());
            controller.ModelState.AddModelError("test1", "message1");

            var result = await controller.GetToken(new Claims());
            result.Should()
                  .BeOfType<BadRequestObjectResult>()
                  .Which.Value.Should()
                  .BeOfType<SerializableError>()
                  .Which.Should()
                  .BeEquivalentTo(new SerializableError(controller.ModelState));
        }

        private UserManager<User> GetUserManagerMock() => Substitute.For<UserManager<User>>(
            Substitute.For<IUserStore<User>, IUserPasswordStore<User>>(),
            Substitute.For<IOptions<IdentityOptions>>(),
            Substitute.For<IPasswordHasher<User>>(), null, null, null, null, null,
            Substitute.For<ILogger<UserManager<User>>>());

        [NotNull]
        private TokenController GetTokenController(UserManager<User> userManager,
            ISecurityTokenWriter securityTokenWriter) => new TokenController(userManager, securityTokenWriter);
    }
}
