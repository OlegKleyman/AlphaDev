using System;
using System.Text;
using AlphaDev.Core;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using Xunit;

namespace AlphaDev.Security.Tests.Unit
{
    public class JwtTokenBuilderFactoryTests
    {
        [Fact]
        public void CreateReturnsJwtTokenBuilderWithValuesFromSettings()
        {
            var dateProvider = Substitute.For<IDateProvider>();
            dateProvider.UtcNow.Returns(new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            var factory = new JwtTokenBuilderFactory(dateProvider);
            var settings = new TokenSettings
            {
                Key = "1234557890987754321",
                Algorithm = SecurityAlgorithms.HmacSha512,
                Audience = nameof(TokenSettings.Audience),
                Issuer = nameof(TokenSettings.Issuer),
                SecondsValid = 10
            };

            var builder = factory.Create(settings);
            builder.Build().Should().BeEquivalentTo(new
            {
                SigningCredentials = new
                {
                    Key = new
                    {
                        Key = Encoding.UTF8.GetBytes(settings.Key)
                    },
                    settings.Algorithm
                },
                settings.Issuer,
                Audiences = new[] {settings.Audience},
                ValidTo = dateProvider.UtcNow.AddSeconds(settings.SecondsValid)
            });
        }
    }
}
