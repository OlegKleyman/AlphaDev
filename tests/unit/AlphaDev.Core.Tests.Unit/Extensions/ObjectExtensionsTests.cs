using System.Threading.Tasks;
using AlphaDev.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Core.Tests.Unit.Extensions
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public async Task ToAsyncValueTaskShouldMapToSpecifiedToFuncReturnValue()
        {
            var result = await new ValueTask<int>(1).ToAsync(i => i.ToString());
            result.Should().Be("1");
        }

        [Fact]
        public void ToShouldMapToSpecifiedToFuncReturnValue()
        {
            1.To(i => i.ToString()).Should().Be("1");
        }

        [Fact]
        public async Task ToAsyncTaskMapsToSpecifiedToFuncReturnValue()
        {
            var result = await Task.FromResult(1).ToAsync(i => i.ToString());
            result.Should().Be("1");
        }
    }
}