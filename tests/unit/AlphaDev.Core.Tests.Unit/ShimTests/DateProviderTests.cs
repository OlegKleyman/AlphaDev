using System;
using FluentAssertions;
using JetBrains.Annotations;
using Pose;
using Xunit;

namespace AlphaDev.Core.Tests.Unit.ShimTests
{
    public class DateProviderTests
    {
        [NotNull]
        private DateProvider GetDateProvider() => new DateProvider();

        [Fact]
        public void UtcNowShouldReturnCurrentTimeInUtc()
        {
            var shim = Shim.Replace(() => DateTime.UtcNow).With(() => new DateTime(2000, 1, 1));
            DateTime? date = null;
            PoseContext.Isolate(() => date = GetDateProvider().UtcNow, shim);
            date.Should().Be(new DateTime(2000, 1, 1));
        }
    }
}
