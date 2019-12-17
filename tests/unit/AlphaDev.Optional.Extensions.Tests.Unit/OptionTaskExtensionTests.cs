using System.Threading.Tasks;
using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class OptionTaskExtensionTests
    {
        [Fact]
        public async Task MatchSomeExecutesSomeExecutesSomeFunctionWithValueWhenResultHasSome()
        {
            var some = new object();
            var optionTask = Task.FromResult(some.Some().WithException(string.Empty));
            object? matchSomeValue = null;
            await optionTask.MatchSomeAsync(o =>
            {
                matchSomeValue = o;
                return Task.CompletedTask;
            });

            matchSomeValue.Should().Be(some);
        }

        [Fact]
        public async Task MatchSomeDoesNotExecutesSomeExecutesSomeFunctionWithValueWhenResultIsNull()
        {
            var optionTask = Task.FromResult(Option.None<object>().WithException(string.Empty));
            var someExecuted = false;
            await optionTask.MatchSomeAsync(o =>
            {
                someExecuted = true;
                return Task.CompletedTask;
            });

            someExecuted.Should().BeFalse();
        }

        [Fact]
        public async Task GetValueOrExceptionAsyncReturnsExceptionValueWhenOptionIsNone()
        {
            var result = await Task.FromResult(Option.None<object>().WithException(() => "test")).GetValueOrExceptionAsync();
            result.Should().Be("test");
        }

        [Fact]
        public async Task GetValueOrExceptionAsyncReturnsSomeValueWhenOptionIsSome()
        {
            var target = new object();
            var result = await Task.FromResult(target.Some().WithException(() => "test")).GetValueOrExceptionAsync();
            result.Should().Be(target);
        }
    }
}
