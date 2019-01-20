using AlphaDev.Web.Extensions;
using FluentAssertions;
using Xunit;
using PositiveInteger = AlphaDev.Web.Support.PositiveInteger;

namespace AlphaDev.Web.Tests.Unit.Extensions
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
