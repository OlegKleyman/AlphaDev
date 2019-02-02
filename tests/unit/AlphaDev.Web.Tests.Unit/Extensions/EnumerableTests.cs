using System.Linq;
using AlphaDev.Web.Extensions;
using AlphaDev.Web.Support;
using FluentAssertions;
using Optional;
using Xunit;
using Enumerable = System.Linq.Enumerable;
using PositiveInteger = AlphaDev.Web.Support.PositiveInteger;

namespace AlphaDev.Web.Tests.Unit.Extensions
{
    public class EnumerableTests
    {
        [Fact]
        public void ToPagerShouldReturnPagerAsConstructor()
        {
            var collection = Enumerable.Range(0, 10).ToArray();
            const int total = 1;
            var pageDimensions = new PageDimensions(PositiveInteger.MinValue, PageBoundaries.MinValue);
            var pager = collection.ToPager(pageDimensions, total.ToPositiveInteger());
            var expectations = new Pager<int>(collection, PageDimensions.MinValue, 1.ToPositiveInteger());
            pager.AuxiliaryPage.Should().Be(expectations.AuxiliaryPage);
            pager.NextPages.Should().BeEquivalentTo(expectations.NextPages);
            pager.Should().BeEquivalentTo(expectations);
        }
    }
}
