using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Core.Extensions;
using AlphaDev.Optional.Extensions;
using AlphaDev.Security;
using AlphaDev.Web.Api.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Optional;
using Optional.Async;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AlphaDev.Web.Api.Controllers
{
    [Route("/token")]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ISecurityTokenWriter _securityTokenWriter;

        public TokenController(UserManager<User> userManager, ISecurityTokenWriter securityTokenWriter)
        {
            _userManager = userManager;
            _securityTokenWriter = securityTokenWriter;
        }

        [Route("")]
        [HttpPost]
        [NotNull]
        public async Task<IActionResult> GetToken([NotNull] [FromBody] Claims claims)
        {
            return await ModelState
                         .SomeWhen(dictionary => dictionary.IsValid,
                             dictionary => (IActionResult) BadRequest(dictionary))
                         .MapAsync(dictionary => _userManager.FindByNameAsync(claims.Username))
                         .MapAsync(user => _userManager.CheckPasswordAsync(user, claims.Password))
                         .FilterAsync(b => b, Unauthorized)
                         .MapAsync(b => new[]
                         {
                             new Claim(JwtRegisteredClaimNames.Sub, claims.Username)
                         })
                         .MapAsync(tokenClaims => _securityTokenWriter.Generate(tokenClaims))
                         .MapAsync(s => (IActionResult) Ok(s))
                         .ValueOrExceptionAsync();
        }
    }
}
