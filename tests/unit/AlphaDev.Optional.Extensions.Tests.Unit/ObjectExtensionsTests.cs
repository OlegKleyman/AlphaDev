using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void SomeWhenNotNullReturnNoneWhenObjectIsNull()
        {
            ((object?) null).SomeWhenNotNull().Should().Be(Option.None<object>());
        }

        [Fact]
        public void SomeWhenNotNullReturnSomeWhenObjectIsNotNull()
        {
            "test".SomeWhenNotNull().Should().Be("test".Some());
        }

        [Fact]
        public void SomeWhenNotNullWithExceptionReturnNoneWithExceptionObjectWhenTargetObjectIsNull()
        {
            ((string?) null).SomeWhenNotNull(() => "exception").ValueOrException().Should().Be("exception");
        }

        [Fact]
        public void SomeWhenNotNullWithExceptionReturnSomeWhenTargetObjectIsNotNull()
        {
            "test".SomeWhenNotNull(() => "exception").ValueOrException().Should().Be("test");
        }
    }
}