using FluentAssertions;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
    public class UnitTests
    {
        [Fact]
        public void ValueReturnsSingletonUnitObject()
        {
            Services.Unit.Value.Should().Be(Services.Unit.Value);
        }
    }
}