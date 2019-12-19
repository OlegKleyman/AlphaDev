using FluentAssertions.Optional.Extensions;
using Optional;
using Xunit;

namespace FluentAssertions.Optional.Tests.Unit.Extensions
{
    public class OptionMaybeTests
    {
        [Fact]
        public void ShouldReturnsOptionMaybeAssertions()
        {
            OptionMaybe.Should(Option.Some(default(object)))
                       .Should()
                       .NotBeNull();
        }
    }
}