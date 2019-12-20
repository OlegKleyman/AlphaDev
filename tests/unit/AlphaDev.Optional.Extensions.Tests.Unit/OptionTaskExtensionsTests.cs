using System.Threading.Tasks;
using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class OptionTaskExtensionsTests
    {
        [Fact]
        public async Task GetValueOrExceptionAsyncReturnsExceptionValueWhenOptionIsNone()
        {
            var result = await Task.FromResult(Option.None<object>().WithException(() => "test"))
                                   .GetValueOrExceptionAsync();
            result.Should().Be("test");
        }

        [Fact]
        public async Task GetValueOrExceptionAsyncReturnsSomeValueWhenOptionIsSome()
        {
            var target = new object();
            var result = await Task.FromResult(target.Some().WithException(() => "test")).GetValueOrExceptionAsync();
            result.Should().Be(target);
        }

        [Fact]
        public static async Task MatchSomeAsyncActionDoesNotExecuteActionWhenNone()
        {
            var optionTask = Task.FromResult(Option.None<object>().WithException(string.Empty));
            var someExecuted = false;
            await optionTask.MatchSomeAsync(o => someExecuted = true);

            someExecuted.Should().BeFalse();
        }

        [Fact]
        public async Task MatchSomeAsyncActionExecutesSomeExecutesSomeFunctionWithValueWhenResultHasSome()
        {
            var some = new object();
            var optionTask = Task.FromResult(some.Some().WithException(string.Empty));
            object? matchSomeValue = null;
            await optionTask.MatchSomeAsync(o => matchSomeValue = o);

            matchSomeValue.Should().Be(some);
        }

        [Fact]
        public async Task MatchSomeAsyncDoesNotExecutesSomeExecutesSomeFunctionWithValueWhenResultIsNull()
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
        public async Task MatchSomeAsyncExecutesSomeExecutesSomeFunctionWithValueWhenResultHasSome()
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
        public static async Task ValueOrAsyncReturnsExceptionWhenOptionHasNone()
        {
            var result = await Task.FromResult(Option.None<string, string>("ex")).ValueOrAsync(s => s);
            result.Should().Be("ex");
        }

        [Fact]
        public static async Task ValueOrAsyncReturnsValueWhenOptionHasSome()
        {
            var result = await Task.FromResult("test".Some().WithException(default(string?)))
                                   .ValueOrAsync(s => string.Empty);
            result.Should().Be("test");
        }
    }
}