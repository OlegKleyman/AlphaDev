using System.Linq;
using AlphaDev.Core;
using AlphaDev.Core.Extensions;
using AlphaDev.Web.Core.Extensions;
using AlphaDev.Web.Core.Support;
using FluentAssertions;
using Xunit;
using Enumerable = System.Linq.Enumerable;

namespace AlphaDev.Web.Core.Tests.Unit.Extensions
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