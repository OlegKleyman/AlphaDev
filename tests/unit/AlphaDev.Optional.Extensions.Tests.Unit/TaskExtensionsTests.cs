using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Optional.Extensions;
using Xunit;

namespace AlphaDev.Optional.Extensions.Tests.Unit
{
    public class TaskExtensionsTests
    {
        [Fact]
        public async Task SomeNotEmptyAsyncReturnsSomeWhenEnumerableIsNotEmpty()
        {
            var enumerable = new[] { 1 }.AsEnumerable();
            var result = await Task.FromResult(enumerable).SomeNotEmptyAsync(() => new object());
            // ReSharper disable once PossibleMultipleEnumeration - enumerable is an array
            result.Should().HaveSome().Which.Should().BeSameAs(enumerable);
        }

        [Fact]
        public async Task SomeNotEmptyAsyncReturnsNoneWhenEnumerableIsEmpty()
        {
            var exception = new object();
            var result = await Task.FromResult(Enumerable.Empty<int>()).SomeNotEmptyAsync(() => exception);
            result.Should().BeNone().Which.Should().BeSameAs(exception);
        }
    }
}
