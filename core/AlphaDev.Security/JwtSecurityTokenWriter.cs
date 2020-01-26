using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AlphaDev.Core;
using AlphaDev.Core.Extensions;
using AlphaDev.Optional.Extensions;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Optional;
using Optional.Unsafe;

namespace AlphaDev.Security
{
    public class JwtSecurityTokenWriter : ISecurityTokenWriter
    {
        private readonly IJwtTokenBuilder _builder;
        private readonly SecurityTokenHandler _handler;

        public JwtSecurityTokenWriter(IJwtTokenBuilder builder, SecurityTokenHandler handler)
        {
            _builder = builder;
            _handler = handler;
        }

        public string Generate(Claim[] claims)
        {
            return _builder.WithClaims(claims).Build()
                           .To(securityToken => _handler.WriteToken(securityToken));
        }
    }
}