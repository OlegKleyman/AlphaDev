using AlphaDev.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Core.Tests.Unit.Extensions
{
    public class Int32Tests
    {
        [Fact]
        public void ToPositiveIntegerShouldReturnPositiveIntegerRepresentationOfInt32Value()
        {
            const int testValue = int.MaxValue;
            testValue.ToPositiveInteger().Should().BeEquivalentTo(new PositiveInteger(testValue));
        }
    }
}