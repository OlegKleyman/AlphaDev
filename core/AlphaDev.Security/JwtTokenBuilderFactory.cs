using System;
using AlphaDev.Core;
using AlphaDev.Optional.Extensions;
using JetBrains.Annotations;
using Microsoft.IdentityModel.Tokens;

namespace AlphaDev.Security
{
    public class JwtTokenBuilderFactory : IJwtTokenBuilderFactory
    {
        private readonly IDateProvider _dateProvider;

        public JwtTokenBuilderFactory(IDateProvider dateProvider) => _dateProvider = dateProvider;

        public IJwtTokenBuilder Create([NotNull] TokenSettings settings)
        {
            IJwtTokenBuilder builder = new JwtTokenBuilder();
            settings.Key.SomeWhenNotNull().MatchSome(s =>
            {
                if (string.IsNullOrWhiteSpace(settings.Algorithm))
                {
                    throw new ArgumentException(
                        $"{nameof(settings.Algorithm)} is null or white space when Key has a value.", nameof(settings));
                }

                builder = builder.WithSigningKey((s, settings.Algorithm));
            });

            return builder.ValidTo(_dateProvider.UtcNow.AddSeconds(settings.SecondsValid))
                          .WithAudience(settings.Audience)
                          .WithIssuer(settings.Issuer);
        }
    }
}
