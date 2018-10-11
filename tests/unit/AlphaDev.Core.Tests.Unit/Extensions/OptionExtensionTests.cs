using AlphaDev.Core.Extensions;
using FluentAssertions;
using Optional;
using Xunit;

namespace AlphaDev.Core.Tests.Unit.Extensions
{
    public class OptionExtensionTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void MapToActionOnSingleValuedOptionWithNoActionParametersShouldExecuteActionWhenSome(bool some, bool expected)
        {
            var option = default(object).SomeWhen(o => some);
            var flag = false;
            option.MapToAction(() => flag = true);
            flag.Should().Be(expected);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void MapToActionOnWithExceptionValuedOptionWithNoActionParametersShouldExecuteActionWhenSome(bool some, bool expected)
        {
            var option = default(object).SomeWhen(o => some, default(object));
            var flag = false;
            option.MapToAction(() => flag = true);
            flag.Should().Be(expected);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void MapToActionOnSingleValuedOptionWithValueActionParameterShouldExecuteActionWhenSome(bool some, bool expected)
        {
            var option = default(object).SomeWhen(o => some);
            var flag = false;
            option.MapToAction(x => flag = true);
            flag.Should().Be(expected);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void MapToActionOnWithExceptionValuedOptionWithValueActionParameterShouldExecuteActionWhenSome(bool some, bool expected)
        {
            var option = default(object).SomeWhen(o => some, default(object));
            var flag = false;
            option.MapToAction(x => flag = true);
            flag.Should().Be(expected);
        }

        [Fact]
        public void MapToActionOnSingleValuedOptionWithNoActionParametersShouldReturnOption()
        {
            var option = default(object).Some();
            option.MapToAction(() => { }).Should().Be(option);
        }

        [Fact]
        public void MapToActionOnSingleValuedOptionWithValueActionParameterShouldReturnOption()
        {
            var option = default(object).Some();
            option.MapToAction(x => { }).Should().Be(option);
        }

        [Fact]
        public void MapToActionOnWithExceptionWithExceptionValuedOptionWithNoActionParametersShouldReturnOption()
        {
            var option = default(object).Some().WithException(default(object));
            option.MapToAction(() => { }).Should().Be(option);
        }

        [Fact]
        public void MapToActionOnWithExceptionWithExceptionValuedOptionWithValueActionParameterShouldReturnOption()
        {
            var option = default(object).Some().WithException(default(object));
            option.MapToAction(x => { }).Should().Be(option);
        }
    }
}
