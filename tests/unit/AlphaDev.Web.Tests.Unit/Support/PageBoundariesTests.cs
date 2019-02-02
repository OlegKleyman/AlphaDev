using AlphaDev.Core.Extensions;
using AlphaDev.Web.Extensions;
using AlphaDev.Web.Support;
using FluentAssertions;
using Xunit;
using PositiveInteger = AlphaDev.Core.PositiveInteger;

namespace AlphaDev.Web.Tests.Unit.Support
{
    public class PageBoundariesTests
    {
        [Fact]
        public void ConstructorShouldInitializeProperties()
        {
            var count = PositiveInteger.MinValue;
            var total = PositiveInteger.MaxValue;
            var boundaries = new PageBoundaries(count, total);
            boundaries.Should().BeEquivalentTo(new {Count = count, MaxTotal = total});
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(1, 2, 1)]
        [InlineData(2, 1, 2)]
        public void GetTotalPagesShouldReturnItemCountDividedByCount(int itemCount, int count, int expected)
        {
            var boundaries = GetPageBoundaries(count.ToPositiveInteger(), default);
            boundaries.GetTotalPages(itemCount).Should().Be(expected);
        }

        [Fact]
        public void MinValueShouldBeCorrect()
        {
            PageBoundaries.MinValue.Should().BeEquivalentTo(new
                { Count = PositiveInteger.MinValue, MaxTotal = PositiveInteger.MinValue });
        }

        private PageBoundaries GetPageBoundaries(PositiveInteger count, PositiveInteger maxTotal)
        {
            return new PageBoundaries(count, maxTotal);
        }
    }
}
