using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JetBrains.Annotations;
using Microsoft.IdentityModel.Tokens;

namespace AlphaDev.Security
{
    public interface IJwtTokenBuilder
    {
        IJwtTokenBuilder WithSigningKey((string key, string algorithm)? key);

        IJwtTokenBuilder WithIssuer(string? issuer);

        IJwtTokenBuilder WithAudience(string? audience);

        IJwtTokenBuilder ValidFrom(DateTime? validFrom);

        IJwtTokenBuilder ValidTo(DateTime? validTo);

        JwtSecurityToken Build();

        IJwtTokenBuilder WithClaims(IEnumerable<Claim> claims);
    }
}