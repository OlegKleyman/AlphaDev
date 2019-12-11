using System;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace AlphaDev.Core.Tests.Integration
{
    public class DateProviderTests
    {
        [NotNull]
        private DateProvider GetDateProvider() => new DateProvider();

        [Fact]
        public void UtcNowShouldReturnCurrentTimeInUtc()
        {
            var provider = GetDateProvider();

            provider.UtcNow.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(1));
        }
    }
}