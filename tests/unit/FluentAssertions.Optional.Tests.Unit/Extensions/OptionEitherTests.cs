using FluentAssertions.Optional.Extensions;
using Optional;
using Xunit;

namespace FluentAssertions.Optional.Tests.Unit.Extensions
{
    public class OptionEitherTests
    {
        [Fact]
        public void ShouldReturnsOptionEitherAssertions()
        {
            OptionEither.Should(Option.Some<object?, object>(default))
                        .Should()
                        .NotBeNull();
        }
    }
}