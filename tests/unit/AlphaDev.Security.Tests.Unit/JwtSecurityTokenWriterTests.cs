using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using Xunit;

namespace AlphaDev.Security.Tests.Unit
{
    public class JwtSecurityTokenWriterTests
    {
        [Fact]
        public void GenerateGeneratesTokenBasedOffClaimsArgument()
        {
            var builder = Substitute.For<IJwtTokenBuilder>();
            var securityTokenHandler = Substitute.For<SecurityTokenHandler>();
            
            var claims = new []{new Claim("",""), };
            builder.WithClaims(claims).Returns(builder);
            var token = new JwtSecurityToken();
            builder.Build().Returns(token);
            
            securityTokenHandler.WriteToken(token).Returns("test");

            var writer = new JwtSecurityTokenWriter(builder, securityTokenHandler);
            var result = writer.Generate(claims);
            result.Should().Be("test");
        }
    }
}
