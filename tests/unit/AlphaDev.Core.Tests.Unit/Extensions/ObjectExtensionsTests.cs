using AlphaDev.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Core.Tests.Unit.Extensions
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void MapShouldMapToSpecifiedToFuncReturnValue()
        {
            1.Map(i => i.ToString()).Should().Be("1");
        }
    }
}