using AlphaDev.Core.Extensions;
using AlphaDev.Web.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Web.Core.Tests.Unit.Extensions
{
    public class PositiveIntegerTests
    {
        [Theory]
        [InlineData(1, 10, 1)]
        [InlineData(2, 10, 11)]
        [InlineData(100, 10, 991)]
        public void ToStartPositionShouldReturnTheStartPositionBasedOnTheItemCount(int page, int itemCount,
            int expected)
        {
            page.ToPositiveInteger()
                .ToStartPosition(itemCount.ToPositiveInteger())
                .Should()
                .Be(expected.ToPositiveInteger());
        }
    }
}