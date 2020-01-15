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
            BlogServices.Core.Unit.Value.Should().Be(BlogServices.Core.Unit.Value);
        }
    }
}