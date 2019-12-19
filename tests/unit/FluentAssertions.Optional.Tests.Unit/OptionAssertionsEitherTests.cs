using System;
using Optional;
using Xunit;

namespace FluentAssertions.Optional.Tests.Unit
{
    public class OptionAssertionsEitherTests
    {
        [Fact]
        public void BeNoneReturnsAndWhichConstraintWithExceptionValueWhenOptionIsNone()
        {
            var option = Option.None<string, string>("test");
            var assertions = new OptionAssertions<string, string>(option);
            assertions.BeNone().Which.Should().Be("test");
        }

        [Fact]
        public void BeNoneThrowsExceptionWhenOptionHasSome()
        {
            var assertions = new OptionAssertions<string?, string?>(Option.Some<string?, string?>(default));
            Action haveSome = () => assertions.BeNone();
            haveSome.Should().Throw<Exception>().WithMessage("Option has a value.");
        }

        [Fact]
        public void HaveSomeReturnsAndWhichConstraintWithSomeValue()
        {
            var option = Option.Some<string, string>("test");
            var assertions = new OptionAssertions<string, string>(option);
            assertions.HaveSome().Which.Should().Be("test");
        }

        [Fact]
        public void HaveSomeThrowsExceptionWhenOptionHasNone()
        {
            var assertions = new OptionAssertions<string, string?>(Option.None<string, string?>(default));
            Action haveSome = () => assertions.HaveSome();
            haveSome.Should().Throw<Exception>().WithMessage("Option does not have a value.");
        }

        [Fact]
        public void SubjectReturnsOptionUsedInConstructor()
        {
            var option = Option.Some<string, string>(string.Empty);
            var assertions = new OptionAssertions<string, string>(option);
            assertions.Subject.Should().Be(option);
        }
    }
}