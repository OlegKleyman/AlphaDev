﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AlphaDev.Web.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Optional;
using Xunit;
using AlphaDev.Web.Extensions;
using Enumerable = System.Linq.Enumerable;
using PositiveInteger = AlphaDev.Web.Support.PositiveInteger;

namespace AlphaDev.Web.Tests.Unit.Support
{
    public class PagerTests
    {
        [Fact]
        public void GetEnumeratorOfTShouldGetEnumeratorOfEnumerable()
        {
            var testValues = Enumerable.Range(0, 10).ToArray();
            var pager = GetPager(testValues, int.MaxValue, int.MaxValue);

            using (var enumerator = pager.GetEnumerator())
            {
                foreach (var testValue in testValues)
                {
                    enumerator.MoveNext().Should().BeTrue();
                    enumerator.Current.Should().Be(testValue);
                }

                enumerator.MoveNext().Should().BeFalse();
            }
        }

        [Fact]
        public void GetEnumeratorOfTShouldNotEnumeratePassedTheMaxItemCount()
        {
            var testValues = Enumerable.Range(0, 10).ToArray();
            var pager = GetPager(testValues, 10, 4);

            using (var enumerator = pager.GetEnumerator())
            {
                for (var i = 0; i < 4; i++)
                {
                    enumerator.MoveNext().Should().BeTrue();
                    enumerator.Current.Should().Be(testValues[i]);
                }

                enumerator.MoveNext().Should().BeFalse();
            }
        }

        [Fact]
        public void GetEnumeratorOfTShouldNotEnumeratePassedThePagesRemaining()
        {
            var testValues = Enumerable.Range(0, 10).ToArray();
            var pager = new Pager<int>(testValues, new PageDimensions(8.ToPositiveInteger(), new PageBoundaries(10.ToPositiveInteger(), 10.ToPositiveInteger())), 73);

            using (var enumerator = pager.GetEnumerator())
            {
                for (var i = 0; i < 3; i++)
                {
                    enumerator.MoveNext().Should().BeTrue();
                    enumerator.Current.Should().Be(testValues[i]);
                }

                enumerator.MoveNext().Should().BeFalse();
            }
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

        [Fact]
        public void ConstructorShouldInitializeWithCollectionArgument()
        {
            var testValues = Enumerable.Range(0, 10).ToList();
            var pager = new Pager<int>(testValues, new PageDimensions(PositiveInteger.MinValue, new PageBoundaries(10.ToPositiveInteger(), 1.ToPositiveInteger())), int.MaxValue);

            using (var enumerator = pager.GetEnumerator())
            {
                foreach (var testValue in testValues)
                {
                    enumerator.MoveNext().Should().BeTrue();
                    enumerator.Current.Should().Be(testValue);
                }

                enumerator.MoveNext().Should().BeFalse();
            }
        }

        [Fact]
        public void ConstructorShouldInitializePagesWithOneAfterTheFirstPage()
        {
            var testValues = Array.Empty<int>();
            var pager = new Pager<int>(testValues, new PageDimensions(7.ToPositiveInteger(), new PageBoundaries(PositiveInteger.MinValue, 2.ToPositiveInteger())), int.MaxValue);
            pager.Pages.First().Should().Be(pager.FirstPage + 1);
        }

        private static class PagerTestsFixture
        {
            [NotNull]
            public static object[][] ConstructorShouldInitializeWithTheCorrectNumberOfPagesTestCases => new[]
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
        [MemberData(nameof(PagerTestsFixture.ConstructorShouldInitializeWithTheCorrectNumberOfPagesTestCases), MemberType = typeof(PagerTestsFixture))]
        public void ConstructorShouldInitializeWithTheCorrectNumberOfPages(int start, PageBoundaries pageBoundaries, int totalItems, int expected)
        {
            var testValues = Array.Empty<int>();
            var pager = new Pager<int>(testValues, new PageDimensions(start.ToPositiveInteger(), pageBoundaries), totalItems);
            pager.Pages.Should().HaveCount(expected);
        }

        [Theory]
        [InlineData(3, 1, 4)]
        [InlineData(10, 11, 21)]
        [InlineData(1, 1, 2)]
        public void ConstructorShouldInitializeWithTheAuxiliaryPageOneGreaterThanTheLastPageWhenTotalItemsDividedByPageBoundaryCountIsGreaterThanTotalBoundary(int totalPagesToDisplay, int start, int expected)
        {
            var testValues = Array.Empty<int>();
            var pageBoundaries = new PageBoundaries(1.ToPositiveInteger(), totalPagesToDisplay.ToPositiveInteger());
            var pager = new Pager<int>(testValues, new PageDimensions(start.ToPositiveInteger(), pageBoundaries), int.MaxValue);
            pager.AuxiliaryPage.Should().Be(expected.Some());
        }

        [Fact]
        public void ConstructorShouldInitializeWithoutTheAuxiliaryPageWhenThereAreLessOrEqualPagesThanTheMaxPageBoundary()
        {
            var testValues = Array.Empty<int>();
            var pager = new Pager<int>(testValues, new PageDimensions(PositiveInteger.MinValue, new PageBoundaries(1.ToPositiveInteger(), 1.ToPositiveInteger())), 1);
            pager.AuxiliaryPage.Should().Be(Option.None<int>());
        }

        [Theory]
        [InlineData(2)]
        [InlineData(1)]
        public void ConstructorShouldInitializePagesWhenThereArePagesRemaining(int size)
        {
            var testValues = Array.Empty<int>();
            var pager = new Pager<int>(testValues, new PageDimensions(PositiveInteger.MinValue, new PageBoundaries(2.ToPositiveInteger(), size.ToPositiveInteger())), int.MaxValue);
            pager.Pages.Should().HaveCount(size - 1);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(1)]
        public void ConstructorShouldInitializeFirstPage(int startPage)
        {
            var testValues = Array.Empty<int>();
            var start = startPage.ToPositiveInteger();
            var pager = new Pager<int>(testValues, new PageDimensions(start, new PageBoundaries(2.ToPositiveInteger(), PositiveInteger.MaxValue)), int.MaxValue);
            pager.FirstPage.Should().Be(start);
        }

        [NotNull]
        private static Pager<T> GetPager<T>([NotNull] ICollection<T> testValues, int total = default, int maxItemsPerPage = 1)
        {
            return new Pager<T>(testValues, new PageDimensions(PositiveInteger.MinValue, new PageBoundaries(maxItemsPerPage.ToPositiveInteger(), 1.ToPositiveInteger())), total);
        }
    }
}
