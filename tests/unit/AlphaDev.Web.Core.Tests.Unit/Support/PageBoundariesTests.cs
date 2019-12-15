using AlphaDev.Core;
using AlphaDev.Core.Extensions;
using AlphaDev.Web.Core.Support;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Web.Core.Tests.Unit.Support
{
    public class PageBoundariesTests
    {
        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(1, 2, 1)]
        [InlineData(2, 1, 2)]
        public void GetTotalPagesShouldReturnItemCountDividedByCount(int itemCount, int count, int expected)
        {
            var boundaries = GetPageBoundaries(count.ToPositiveInteger(), PositiveInteger.MinValue);
            boundaries.GetTotalPages(itemCount).Should().Be(expected);
        }

        private PageBoundaries GetPageBoundaries(PositiveInteger count, PositiveInteger maxTotal) =>
            new PageBoundaries(count, maxTotal);

        [Fact]
        public void ConstructorShouldInitializeProperties()
        {
            var count = PositiveInteger.MinValue;
            var total = PositiveInteger.MaxValue;
            var boundaries = new PageBoundaries(count, total);
            boundaries.Should().BeEquivalentTo(new { Count = count, MaxTotal = total });
        }

        [Fact]
        public void MinValueShouldBeCorrect()
        {
            PageBoundaries.MinValue.Should()
                          .BeEquivalentTo(new
                              { Count = PositiveInteger.MinValue, MaxTotal = PositiveInteger.MinValue });
        }
    }
}