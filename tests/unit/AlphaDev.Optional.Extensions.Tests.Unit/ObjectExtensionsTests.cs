using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void SomeWhenNotNullShouldReturnNoneWhenObjectIsNull()
        {
            ((object?) null).SomeWhenNotNull().Should().Be(Option.None<object>());
        }

        [Fact]
        public void SomeWhenNotNullShouldReturnSomeWhenObjectIsNotNull()
        {
            "test".SomeWhenNotNull().Should().Be("test".Some());
        }

        [Fact]
        public void SomeWhenNotNullWithExceptionShouldReturnNoneWithExceptionObjectWhenTargetObjectIsNull()
        {
            ((string?) null).SomeWhenNotNull(() => "exception").ValueOrException().Should().Be("exception");
        }

        [Fact]
        public void SomeWhenNotNullWithExceptionShouldReturnSomeWhenTargetObjectIsNotNull()
        {
            "test".SomeWhenNotNull(() => "exception").ValueOrException().Should().Be("test");
        }
    }
}
