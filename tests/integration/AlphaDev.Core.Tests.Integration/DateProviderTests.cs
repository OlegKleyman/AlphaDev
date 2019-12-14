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
            var now = DateTime.UtcNow;
            GetDateProvider()
                           .UtcNow.Should()
                           .BeOnOrAfter(now)
                           .And.BeCloseTo(now, TimeSpan.FromMilliseconds(5));
        }
    }
}