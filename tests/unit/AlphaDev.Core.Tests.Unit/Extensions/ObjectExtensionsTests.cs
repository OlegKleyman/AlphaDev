using AlphaDev.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Core.Tests.Unit.Extensions
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void ToShouldMapToSpecifiedToFuncReturnValue()
        {
            1.To(i => i.ToString()).Should().Be("1");
        }
    }
}