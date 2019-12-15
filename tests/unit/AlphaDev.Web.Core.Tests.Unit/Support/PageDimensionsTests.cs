using AlphaDev.Core;
using AlphaDev.Web.Core.Support;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Web.Core.Tests.Unit.Support
{
    public class PageDimensionsTests
    {
        [Fact]
        public void ConstructorShouldInitializeProperties()
        {
            var maxItemCount = PositiveInteger.MinValue;
            var maxPageTotal = PositiveInteger.MaxValue;
            var pageDimensions =
                new PageDimensions(PositiveInteger.MinValue, new PageBoundaries(maxItemCount, maxPageTotal));
            pageDimensions.Should()
                          .BeEquivalentTo(new
                          {
                              Start = PositiveInteger.MinValue,
                              Boundaries = new { Count = maxItemCount, MaxTotal = maxPageTotal }
                          });
        }

        [Fact]
        public void MinValueShouldBeCorrect()
        {
            PageDimensions.MinValue.Should()
                          .BeEquivalentTo(new
                              { Start = PositiveInteger.MinValue, Boundaries = PageBoundaries.MinValue });
        }
    }
}