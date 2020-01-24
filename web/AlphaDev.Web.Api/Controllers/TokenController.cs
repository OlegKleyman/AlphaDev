using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Web.Api.Models;
using AlphaDev.Web.Api.Support;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AlphaDev.Web.Api.Controllers
{
    [Route("/token")]
    public class TokenController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IOptionsSnapshot<TokenSettings> _tokenSettings;

        public TokenController(SignInManager<User> signInManager, IOptionsSnapshot<TokenSettings> tokenSettings)
        {
            _signInManager = signInManager;
            _tokenSettings = tokenSettings;
        }

        [Route("")]
        [HttpPost]
        [NotNull]
        [ItemNotNull]
        public async Task<ActionResult<string>> GetToken([NotNull] [FromBody] Claims claims)
        {
            var result = await _signInManager.PasswordSignInAsync(claims.Username, claims.Password, false, false);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (result == SignInResult.Success)
            {
                var jwtClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, claims.Username)
                };

                var token = new JwtSecurityToken(_tokenSettings.Value.Issuer, _tokenSettings.Value.Audience, jwtClaims,
                    DateTime.UtcNow, DateTime.UtcNow.AddHours(1));

                var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
                return encodedToken;
            }

            return Unauthorized();
        }
    }
}
