using System;
using AlphaDev.Test.Core.Extensions;
using AlphaDev.Web.Support;
using FluentAssertions;
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
        public void ValueShouldThrowInvalidOperationExceptionWhenValueIsLessThanOne()
        {
            var integer = new PositiveInteger();
            Action value = () => integer.Value.EmptyCall();
            value.Should().Throw<InvalidOperationException>().WithMessage("The value is invalid.");
        }

        [Fact]
        public void ValueShouldReturnValue()
        {
            const int value = 1;
            var integer = GetPositiveInteger(value);
            integer.Value.Should().Be(value);
        }

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
    }
}
