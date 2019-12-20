using System.Threading.Tasks;
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

        [Fact]
        public void GetValueOrExceptionReturnsExceptionValueWhenOptionIsNone()
        {
            Option.None<object>().WithException(() => "test").GetValueOrException().Should().Be("test");
        }

        [Fact]
        public void GetValueOrExceptionReturnsSomeValueWhenOptionIsSome()
        {
            var target = new object();
            target.Some().WithException(() => "test").GetValueOrException().Should().Be(target);
        }

        [Fact]
        public async Task MatchSomeAsyncDoesNotExecutesFuncWhenOptionIsNone()
        {
            int? result = null;
            await Option.None<int, int>(1).MatchSomeAsync(i => Task.Run(() => result = i));

            result.Should().BeNull();
        }

        [Fact]
        public async Task MatchSomeAsyncExecutesFuncWhenOptionHasSome()
        {
            int? result = null;

            await 1.Some().WithException(() => 2).MatchSomeAsync(i => Task.Run(() => result = i));

            result.Should().Be(1);
        }
    }
}