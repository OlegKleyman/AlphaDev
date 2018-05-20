using System;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Core.Tests.Integration
{
    public class DateProviderTests
    {
        private DateProvider GetDateProvider()
        {
            return new DateProvider();
        }

        [Fact]
        public void UtcNowShouldReturnCurrentTimeInUtc()
        {
            var provider = GetDateProvider();

            provider.UtcNow.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(.5));
        }
    }
}