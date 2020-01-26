using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AlphaDev.Core.Extensions;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace AlphaDev.Security.Tests.Unit
{
    public class JwtTokenBuilderTests
    {
        [Fact]
        public void WithClaimsBuildsJwtSecurityTokenWithTheClaimaArgument()
        {
            var claims = new[]
            {
                new Claim("type1", "value1"),
                new Claim("type2", "value2"),
                new Claim("type3", "value3")
            };

            JwtTokenBuilder.Start()
                           .WithClaims(claims)
                           .Build()
                           .Claims.Should()
                           .BeEquivalentTo(claims.AsEnumerable());

        }

        [Fact]
        public void WithSigningKeyBuildsJwtSecurityTokenWithSigningCredentialsOfSigningKey()
        {
            const string key = "key1234577890987754321";

            var jwtSecurityToken = JwtTokenBuilder.Start()
                                                  .WithSigningKey((key, SecurityAlgorithms.HmacSha512))
                                                  .Build();
            jwtSecurityToken.To(token => new
            {
                ((SymmetricSecurityKey)token.SigningCredentials.Key).Key,
                token.SigningCredentials.Algorithm
            }).Should().BeEquivalentTo(new
            {
                Algorithm = SecurityAlgorithms.HmacSha512,
                Key = Encoding.UTF8.GetBytes(key)
            });
        }

        [Fact]
        public void WithIssuerBuildsJwtSecurityTokenWithIssuerArgument()
        {
            JwtTokenBuilder.Start().WithIssuer("test").Build().Issuer.Should().Be("test");
        }

        [Fact]
        public void WithAudienceBuildsJwtSecurityTokenWithAudienceArgument()
        {
            JwtTokenBuilder.Start()
                           .WithAudience("test")
                           .Build()
                           .Audiences.Should()
                           .ContainSingle()
                           .Which.Should()
                           .Be("test");
        }

        [Fact]
        public void ValidFromBuildsJwtSecurityTokenWithValidFromArgument()
        {
            var date = new DateTime(2010,1,1);
            var jwtSecurityToken = JwtTokenBuilder.Start()
                                                  .ValidFrom(date)
                                                  .ValidTo(new DateTime(2021, 1, 1))
                                                  .Build();
            jwtSecurityToken.ValidFrom.Date.Should().Be(date);
        }

        [Fact]
        public void ValidToBuildsJwtSecurityTokenWithValidToArgument()
        {
            var date = new DateTime(2010, 1, 1);
            var jwtSecurityToken = JwtTokenBuilder.Start().ValidTo(date).Build();
            jwtSecurityToken.ValidTo.Date.Should().Be(date);
        }

        [Fact]
        public void StartReturnsJwtTokenBuilder()
        {
            JwtTokenBuilder.Start().Should().NotBeNull();
        }

        [Fact]
        public void BuildReturnsJwtSecurityTokenWithoutSigningCredentialsWhenSigningKeyIsNotSet()
        {
            JwtTokenBuilder.Start().Build().SigningCredentials.Should().BeNull();
        }

        [Fact]
        public void BuildReturnsJwtSecurityTokenWithoutSigningCredentialsWhenSigningKeyIsSet()
        {
            const string key = "key1234577890987754321";

            var jwtSecurityToken = JwtTokenBuilder.Start()
                                                  .WithSigningKey((key, SecurityAlgorithms.HmacSha512))
                                                  .Build();
            jwtSecurityToken.To(token => new
            {
                ((SymmetricSecurityKey)token.SigningCredentials.Key).Key,
                token.SigningCredentials.Algorithm
            }).Should().BeEquivalentTo(new
            {
                Algorithm = SecurityAlgorithms.HmacSha512,
                Key = Encoding.UTF8.GetBytes(key)
            });
        }
    }
}
