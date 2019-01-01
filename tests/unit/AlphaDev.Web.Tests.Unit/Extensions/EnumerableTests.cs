using System.Linq;
using AlphaDev.Web.Extensions;
using FluentAssertions;
using Optional;
using Xunit;
using Enumerable = System.Linq.Enumerable;

namespace AlphaDev.Web.Tests.Unit.Extensions
{
    public class EnumerableTests
    {
        [Fact]
        public void ToPagerShouldReturnPagerWithTheSameElements()
        {
            var collection = Enumerable.Range(0, 10).ToArray();
            var pager = collection.ToPager(default, x => false);
            pager.Should().BeEquivalentTo(collection);
        }

        [Fact]
        public void ToPagerShouldReturnPagerWithTheCorrectStartPage()
        {
            var collection = Enumerable.Range(0, 10).ToArray();
            var pager = collection.ToPager(10, x => false);
            pager.Pages.Should().BeEquivalentTo(collection.Select((_, i) => i + 10));
        }

        [Fact]
        public void ToPagerShouldReturnPagerWithTheAuxiliaryPageWhenHasAuxiliaryArgumentIsTrue()
        {
            var collection = Enumerable.Range(0, 10).ToArray();
            var pager = collection.ToPager(10, x => true);
            pager.AuxiliaryPage.Should().BeEquivalentTo((collection.Length - 1 + 10).Some());
        }
    }
}
