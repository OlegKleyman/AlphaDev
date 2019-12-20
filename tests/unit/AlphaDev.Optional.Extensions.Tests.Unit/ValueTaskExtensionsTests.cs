using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Optional.Extensions;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class ValueTaskExtensionsTests
    {
        [Fact]
        public static async Task SomeNotNullAsyncValueTaskReturnsExceptionNoneWhenTargetNull()
        {
            var option =
                await new ValueTask<string?>(Task.FromResult(default(string?))).SomeNotNullAsync(() => "exception");
            option.Should().BeNone().Which.Should().Be("exception");
        }

        [Fact]
        public static async Task SomeNotNullAsyncValueTaskReturnsNoneWhenTargetNull()
        {
            var option = await new ValueTask<object?>(Task.FromResult(default(object?))).SomeNotNullAsync();
            option.Should().BeNone();
        }

        [Fact]
        public static async Task SomeNotNullAsyncValueTaskReturnsSomeWhenTargetNotNull()
        {
            var option = await new ValueTask<string>(Task.FromResult("test")).SomeNotNullAsync();
            option.Should().HaveSome().Which.Should().Be("test");
        }

        [Fact]
        public static async Task SomeNotNullAsyncValueTaskWithExceptionReturnsSomeWhenTargetNotNull()
        {
            var option = await new ValueTask<string>(Task.FromResult("test")).SomeNotNullAsync(() => new object());
            option.Should().HaveSome().Which.Should().Be("test");
        }
    }
}