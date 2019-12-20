using FluentAssertions;
using FluentAssertions.Optional.Extensions;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void SomeWhenNotNullReturnNoneWhenObjectIsNull()
        {
            ((object?) null).SomeWhenNotNull().Should().BeNone();
        }

        [Fact]
        public void SomeWhenNotNullReturnSomeWhenObjectIsNotNull()
        {
            "test".SomeWhenNotNull().Should().HaveSome().Which.Should().Be("test");
        }

        [Fact]
        public void SomeWhenNotNullWithExceptionReturnNoneWithExceptionObjectWhenTargetObjectIsNull()
        {
            ((string?) null).SomeWhenNotNull(() => "exception").Should().BeNone().Which.Should().Be("exception");
        }

        [Fact]
        public void SomeWhenNotNullWithExceptionReturnSomeWhenTargetObjectIsNotNull()
        {
            "test".SomeWhenNotNull(() => "exception").Should().HaveSome().Which.Should().Be("test");
        }
    }
}