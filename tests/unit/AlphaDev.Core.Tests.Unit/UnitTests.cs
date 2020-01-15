using FluentAssertions;
using FluentAssertions.Common;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
    public class UnitTests
    {
        [Fact]
        public void ValueReturnsSingletonUnitObject()
        {
            Core.Unit.Value.Should().Be(Core.Unit.Value);
        }
    }
}