using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class OptionExtensionsTests
    {
        [Fact]
        public void FilterNotNullReturnsNoneWhenOptionContainsNull()
        {
            ((object?) null).Some().FilterNotNull().Should().Be(Option.None<object>());
        }

        [Fact]
        public void FilterNotNullReturnsSomeWhenOptionDoesNotContainNull()
        {
            // ReSharper disable once RedundantCast - need to convert to nullable reference type to use extension method
            ((object?) new object()).Some().FilterNotNull().HasValue.Should().BeTrue();
        }
    }
}