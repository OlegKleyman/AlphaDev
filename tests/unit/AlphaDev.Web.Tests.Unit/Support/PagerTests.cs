using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AlphaDev.Web.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Optional;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Support
{
    public class PagerTests
    {
        [Fact]
        public void GetEnumeratorOfTShouldGetEnumeratorOfEnumerable()
        {
            var testValues = Enumerable.Range(0, 10).ToArray();
            var pager = GetPager(testValues);

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
        public void GetEnumeratorShouldGetEnumeratorOfEnumerable()
        {
            var testValues = Enumerable.Range(0, 10).ToArray();
            IEnumerable pager = GetPager(testValues);
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
            var pager = new Pager<int>(testValues, 1);

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
        public void ConstructorShouldInitializeWithTheStartingPage()
        {
            var testValues = Enumerable.Range(0, 10).ToArray();
            var pager = new Pager<int>(testValues, 7);
            pager.Pages.Should().BeEquivalentTo(testValues.Select((_, i) => i + 7));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        public void ConstructorShouldInitializeWithTheAuxiliaryPageWhenHasAuxiliaryArgumentIsTrueAndCollectionIsGreaterThanOne(int size)
        {
            var testValues = Enumerable.Range(1, size).ToArray();
            var pager = new Pager<int>(testValues, 1, true);
            pager.AuxiliaryPage.Should().Be(testValues.Length.Some());
        }

        [Fact]
        public void ConstructorShouldInitializeWithoutTheAuxiliaryPageWhenHasAuxiliaryArgumentIsFalse()
        {
            var testValues = Enumerable.Range(1, 10).ToArray();
            var pager = new Pager<int>(testValues, 1, false);
            pager.AuxiliaryPage.Should().Be(Option.None<int>());
        }

        [Fact]
        public void ConstructorShouldInitializeWithoutTheAuxiliaryPageWhenHasAuxiliaryArgumentIsTrueAndCollectionHasLessThanTwoElements()
        {
            var testValues = new[] { 1 }.ToArray();
            var pager = new Pager<int>(testValues, 1, true);
            pager.AuxiliaryPage.Should().Be(Option.None<int>());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void ConstructorShouldInitializePagesWhenHasAuxiliaryArgumentIsTrueAndCollectionHasLessThanTwoElements(int size)
        {
            var testValues = Enumerable.Range(1, size).ToArray();
            var pager = new Pager<int>(testValues, 1, true);
            pager.Pages.Should().HaveCount(size);
        }

        [NotNull]
        private static Pager<T> GetPager<T>([NotNull] ICollection<T> testValues)
        {
            return new Pager<T>(testValues, 1);
        }
    }
}
