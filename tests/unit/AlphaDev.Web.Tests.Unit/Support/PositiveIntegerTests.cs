using System;
using AlphaDev.Test.Core.Extensions;
using AlphaDev.Web.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace AlphaDev.Web.Tests.Unit.Support
{
    public class PositiveIntegerTests
    {
        [Fact]
        public void ConstructorShouldInitializeValue()
        {
            const int value = 1;
            var integer = new PositiveInteger(value);
            integer.Value.Should().Be(value);
        }

        [Fact]
        public void ConstructorShouldThrowArgumentExceptionWhenValueIsLessThanOne()
        {
            Action constructor = () => new PositiveInteger(-1).EmptyCall();
            constructor.Should().Throw<ArgumentException>().WithMessage("Must be positive.\r\nParameter name: value")
                .Which.ParamName.Should().Be("value");
        }

        [Fact]
        public void ValueShouldReturnValue()
        {
            const int value = 1;
            var integer = GetPositiveInteger(value);
            integer.Value.Should().Be(value);
        }

        [NotNull]
        private static PositiveInteger GetPositiveInteger(int value)
        {
            return new PositiveInteger(value);
        }

        [Fact]
        public void DivideOperatorShouldDividePositiveIntegerWithInt32()
        {
            const int dividend = 10;
            const int divisor = 2;

            var integer = GetPositiveInteger(dividend);
            (integer / divisor).Should().Be(dividend / divisor);
        }

        [Fact]
        public void DivideOperatorShouldDivideInt32WithPositiveInteger()
        {
            const int dividend = 10;
            const int divisor = 2;

            var integer = GetPositiveInteger(divisor);
            (dividend / integer).Should().Be(dividend / divisor);
        }

        [Fact]
        public void DivideOperatorShouldDividePositiveIntegerWithPositiveInteger()
        {
            var dividend = PositiveInteger.MaxValue;
            var divisor = PositiveInteger.MinValue;

            (dividend / divisor).Should().Be(dividend.Value / divisor.Value);
        }

        [Fact]
        public void AdditionOperatorShouldAddPositiveIntegerWithInt32()
        {
            const int x = 10;
            const int y = 2;

            var integer = GetPositiveInteger(x);
            (integer + y).Should().Be(x + y);
        }

        [Fact]
        public void MinValueShouldBeOne()
        {
            PositiveInteger.MinValue.Value.Should().Be(1);
        }

        [Fact]
        public void MaxValueShouldBeInt32MaxValue()
        {
            PositiveInteger.MaxValue.Value.Should().Be(int.MaxValue);
        }

        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(1, 2, false)]
        public void ObjectEqualsShouldReturnWhetherTwoPositiveIntegersAreEqual(int first, int second, bool expected)
        {
            // ReSharper disable once RedundantCast - object equals method needs to be tested so a cast to object is a must
            ((object) new PositiveInteger(first)).Equals(new PositiveInteger(second)).Should().Be(expected);
        }

        [Fact]
        public void ObjectEqualsMethodShouldReturnFalseWhenOtherObjectIsNotPositiveInteger()
        {
            ((object) PositiveInteger.MinValue).Equals(new object()).Should().BeFalse();
        }

        [Fact]
        public void ObjectEqualsMethodShouldReturnFalseWhenOtherObjectIsNull()
        {
            ((object)PositiveInteger.MinValue).Equals(null).Should().BeFalse();
        }

        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(1, 2, false)]
        public void EqualsShouldReturnWhetherTwoPositiveIntegersAreEqual(int first, int second, bool expected)
        {
            new PositiveInteger(first).Equals(new PositiveInteger(second)).Should().Be(expected);
        }

        [Fact]
        public void EqualsMethodShouldReturnFalseWhenOtherObjectIsNull()
        {
            PositiveInteger.MinValue.Equals(null).Should().BeFalse();
        }
    }
}
