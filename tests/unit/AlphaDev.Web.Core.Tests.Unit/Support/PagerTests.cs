using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AlphaDev.Core;
using AlphaDev.Core.Extensions;
using AlphaDev.Web.Core.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Optional;
using Xunit;

namespace AlphaDev.Web.Core.Tests.Unit.Support
{
    public class PagerTests
    {
        private static class PagerTestsFixture
        {
            [NotNull]
            public static object[][] ConstructorShouldInitializeWithTheCorrectNumberOfNextPagesTestCases =>
                new[]
                {
                    new object[]
                    {
                        1,
                        new PageBoundaries(10.ToPositiveInteger(), 10.ToPositiveInteger()),
                        200,
                        9
                    },
                    new object[]
                    {
                        1,
                        new PageBoundaries(10.ToPositiveInteger(), 10.ToPositiveInteger()),
                        7,
                        0
                    },
                    new object[]
                    {
                        8,
                        new PageBoundaries(10.ToPositiveInteger(), 10.ToPositiveInteger()),
                        7,
                        0
                    },
                    new object[]
                    {
                        8,
                        new PageBoundaries(3.ToPositiveInteger(), 5.ToPositiveInteger()),
                        7,
                        2
                    }
                };
        }

        [Theory]
        [MemberData(nameof(PagerTestsFixture.ConstructorShouldInitializeWithTheCorrectNumberOfNextPagesTestCases),
            MemberType = typeof(PagerTestsFixture))]
        public void ConstructorShouldInitializeWithTheCorrectNumberOfNextPages(int start, PageBoundaries pageBoundaries,
            int totalItems, int expected)
        {
            var testValues = Array.Empty<int>();
            var pager = new Pager<int>(testValues, new PageDimensions(start.ToPositiveInteger(), pageBoundaries),
                totalItems.ToPositiveInteger());
            pager.NextPages.Should().HaveCount(expected);
        }

        [Theory]
        [InlineData(3, 1, 4)]
        [InlineData(10, 11, 21)]
        [InlineData(1, 1, 2)]
        public void
            ConstructorShouldInitializeWithTheAuxiliaryPageOneGreaterThanTheLastPageWhenTotalItemsDividedByPageBoundaryCountIsGreaterThanTotalBoundary(
                int totalPagesToDisplay, int start, int expected)
        {
            var testValues = Array.Empty<int>();
            var pageBoundaries = new PageBoundaries(1.ToPositiveInteger(), totalPagesToDisplay.ToPositiveInteger());
            var pager = new Pager<int>(testValues, new PageDimensions(start.ToPositiveInteger(), pageBoundaries),
                PositiveInteger.MaxValue);
            pager.AuxiliaryPage.Should().Be(expected.Some());
        }

        [Theory]
        [InlineData(2)]
        [InlineData(1)]
        public void ConstructorShouldInitializeNextPagesWhenThereArePagesRemaining(int size)
        {
            var testValues = Array.Empty<int>();
            var pager = new Pager<int>(testValues,
                new PageDimensions(PositiveInteger.MinValue,
                    new PageBoundaries(2.ToPositiveInteger(), size.ToPositiveInteger())), PositiveInteger.MaxValue);
            pager.NextPages.Should().HaveCount(size - 1);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(1)]
        public void ConstructorShouldInitializeCurrentPage(int startPage)
        {
            var testValues = Array.Empty<int>();
            var start = startPage.ToPositiveInteger();
            var pager = new Pager<int>(testValues,
                new PageDimensions(start, new PageBoundaries(2.ToPositiveInteger(), 1000.ToPositiveInteger())),
                PositiveInteger.MaxValue);
            pager.CurrentPage.Should().Be(start);
        }

        [Theory]
        [InlineData(6)]
        [InlineData(5)]
        [InlineData(2)]
        [InlineData(1)]
        [InlineData(8)]
        [InlineData(11)]
        public void ConstructorShouldInitializePreviousPagesWithUpToFiveBeforeTheCurrentPage(int startPage)
        {
            const int maxTotalPagesToDisplay = 5;
            var testValues = Array.Empty<int>();
            var start = startPage.ToPositiveInteger();
            var pager = new Pager<int>(testValues,
                new PageDimensions(start,
                    new PageBoundaries(2.ToPositiveInteger(), maxTotalPagesToDisplay.ToPositiveInteger())),
                PositiveInteger.MaxValue);
            pager.CurrentPage.Should().Be(start);
            var previousPages = Enumerable.Range(startPage - maxTotalPagesToDisplay, 5).Where(x => x > 0);
            pager.PreviousPages.Should().BeEquivalentTo(previousPages);
        }

        [NotNull]
        private static Pager<T> GetPager<T>([NotNull] ICollection<T> testValues, int total = default,
            int maxItemsPerPage = 1) => new Pager<T>(testValues,
            new PageDimensions(PositiveInteger.MinValue,
                new PageBoundaries(maxItemsPerPage.ToPositiveInteger(), 1.ToPositiveInteger())),
            total.ToPositiveInteger());

        [Fact]
        public void ConstructorShouldInitializeNextPagesWithOneAfterTheCurrentPage()
        {
            var testValues = Array.Empty<int>();
            var pager = new Pager<int>(testValues,
                new PageDimensions(7.ToPositiveInteger(),
                    new PageBoundaries(PositiveInteger.MinValue, 2.ToPositiveInteger())), PositiveInteger.MaxValue);
            pager.NextPages.First().Should().Be(pager.CurrentPage + 1);
        }

        [Fact]
        public void ConstructorShouldInitializeWithCollectionArgument()
        {
            var testValues = Enumerable.Range(0, 10).ToList();
            var pager = new Pager<int>(testValues,
                new PageDimensions(PositiveInteger.MinValue,
                    new PageBoundaries(10.ToPositiveInteger(), 1.ToPositiveInteger())), PositiveInteger.MaxValue);

            using var enumerator = pager.GetEnumerator();
            foreach (var testValue in testValues)
            {
                enumerator.MoveNext().Should().BeTrue();
                enumerator.Current.Should().Be(testValue);
            }

            enumerator.MoveNext().Should().BeFalse();
        }

        [Fact]
        public void
            ConstructorShouldInitializeWithoutTheAuxiliaryPageWhenThereAreLessOrEqualPagesThanTheMaxPageBoundary()
        {
            var testValues = Array.Empty<int>();
            var pager = new Pager<int>(testValues,
                new PageDimensions(PositiveInteger.MinValue,
                    new PageBoundaries(1.ToPositiveInteger(), 1.ToPositiveInteger())), 1.ToPositiveInteger());
            pager.AuxiliaryPage.Should().Be(Option.None<int>());
        }

        [Fact]
        public void GetEnumeratorOfTShouldGetEnumeratorOfEnumerable()
        {
            var testValues = Enumerable.Range(0, 10).ToArray();
            var pager = GetPager(testValues, int.MaxValue, int.MaxValue);

            using var enumerator = pager.GetEnumerator();
            foreach (var testValue in testValues)
            {
                enumerator.MoveNext().Should().BeTrue();
                enumerator.Current.Should().Be(testValue);
            }

            enumerator.MoveNext().Should().BeFalse();
        }

        [Fact]
        public void GetEnumeratorOfTShouldNotEnumeratePassedTheBoundaryCount()
        {
            var testValues = Enumerable.Range(0, 10).ToArray();
            var pager = new Pager<int>(testValues,
                new PageDimensions(8.ToPositiveInteger(),
                    new PageBoundaries(7.ToPositiveInteger(), 10.ToPositiveInteger())), 73.ToPositiveInteger());

            using var enumerator = pager.GetEnumerator();
            for (var i = 0; i < 7; i++)
            {
                enumerator.MoveNext().Should().BeTrue();
                enumerator.Current.Should().Be(testValues[i]);
            }

            enumerator.MoveNext().Should().BeFalse();
        }

        [Fact]
        public void GetEnumeratorOfTShouldNotEnumeratePassedTheMaxItemCount()
        {
            var testValues = Enumerable.Range(0, 10).ToArray();
            var pager = GetPager(testValues, 10, 4);

            using var enumerator = pager.GetEnumerator();
            for (var i = 0; i < 4; i++)
            {
                enumerator.MoveNext().Should().BeTrue();
                enumerator.Current.Should().Be(testValues[i]);
            }

            enumerator.MoveNext().Should().BeFalse();
        }

        [Fact]
        public void GetEnumeratorShouldGetEnumeratorOfEnumerable()
        {
            var testValues = Enumerable.Range(0, 10).ToArray();
            IEnumerable pager = GetPager(testValues, int.MaxValue, int.MaxValue);
            var enumerator = pager.GetEnumerator();

            foreach (var testValue in testValues)
            {
                enumerator.MoveNext().Should().BeTrue();
                enumerator.Current.Should().Be(testValue);
            }

            enumerator.MoveNext().Should().BeFalse();
        }
    }
}