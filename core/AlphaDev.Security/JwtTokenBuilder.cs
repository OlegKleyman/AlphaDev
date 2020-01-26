using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AlphaDev.Core.Extensions;
using JetBrains.Annotations;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Optional;
using Optional.Unsafe;

namespace AlphaDev.Security
{
    public class JwtTokenBuilder : IJwtTokenBuilder
    {
        private string? _issuer;
        private string? _audience;
        private ImmutableArray<Claim> _claims;
        private DateTime? _validFrom;
        private DateTime? _validTo;
        private (string key, string algorithm)? _keyWithAlgorithm;

        public JwtTokenBuilder() => _claims = ImmutableArray<Claim>.Empty;

        [NotNull]
        public static IJwtTokenBuilder Start() => new JwtTokenBuilder();

        [NotNull]
        public IJwtTokenBuilder WithSigningKey((string key, string algorithm)? key)
        {
            return CloneAnd(builder => builder._keyWithAlgorithm = key);
        }

        [NotNull]
        public IJwtTokenBuilder WithIssuer(string? issuer)
        {
            return CloneAnd(builder => builder._issuer = issuer);
        }

        [NotNull]
        public IJwtTokenBuilder WithAudience(string? audience)
        {
            return CloneAnd(builder => builder._audience = audience);
        }

        [NotNull]
        public IJwtTokenBuilder ValidFrom(DateTime? validFrom)
        {
            return CloneAnd(builder => builder._validFrom = validFrom);
        }

        [NotNull]
        public IJwtTokenBuilder ValidTo(DateTime? validTo)
        {
            return CloneAnd(builder => builder._validTo = validTo);
        }

        [NotNull]
        private IJwtTokenBuilder CloneAnd([NotNull] Action<JwtTokenBuilder> action)
        {
            var clone = (JwtTokenBuilder)MemberwiseClone();
            action(clone);
            return clone;
        }

        [NotNull]
        public JwtSecurityToken Build()
        {
            return _keyWithAlgorithm.ToOption()
                                    .Map(s => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(s.key)))
                                    .Map(securityKey =>
                                        // ReSharper disable once PossibleInvalidOperationException - _keyWithAlgorithm will
                                        // always have a value here because the option has some
                                        new SigningCredentials(securityKey, _keyWithAlgorithm!.Value.algorithm))
                                    .ValueOrDefault()
                                    .To(credentials => new JwtSecurityToken(_issuer, _audience, _claims, _validFrom,
                                        _validTo, credentials));
        }

        [NotNull]
        public IJwtTokenBuilder WithClaims(IEnumerable<Claim> claims)
        {
            return CloneAnd(builder => builder._claims = claims.ToImmutableArray());
        }
    }
}