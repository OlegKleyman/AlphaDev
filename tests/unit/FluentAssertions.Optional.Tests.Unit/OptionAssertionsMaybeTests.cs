using System;
using Optional;
using Xunit;

namespace FluentAssertions.Optional.Tests.Unit
{
    public class OptionAssertionsMaybeTests
    {
        [Fact]
        public void BeNoneReturnsAndConstraintWithSelf()
        {
            var assertions = new OptionAssertions<string>(Option.None<string>());
            assertions.BeNone().And.Should().Be(assertions);
        }

        [Fact]
        public void BeNoneThrowsExceptionWhenOptionHasSome()
        {
            var assertions = new OptionAssertions<string?>(default(string).Some());
            Action haveSome = () => assertions.BeNone();
            haveSome.Should().Throw<Exception>().WithMessage("Option has a value.");
        }

        [Fact]
        public void HaveSomeReturnsAndWhichConstraintWithSomeValue()
        {
            var option = "test".Some();
            var assertions = new OptionAssertions<string>(option);
            assertions.HaveSome().Which.Should().Be("test");
        }

        [Fact]
        public void HaveSomeThrowsExceptionWhenOptionHasNone()
        {
            var assertions = new OptionAssertions<string>(Option.None<string>());
            Action haveSome = () => assertions.HaveSome();
            haveSome.Should().Throw<Exception>().WithMessage("Option does not have a value.");
        }

        [Fact]
        public void SubjectReturnsOptionUsedInConstructor()
        {
            var option = "test".Some();
            var assertions = new OptionAssertions<string>(option);
            assertions.Subject.Should().Be(option);
        }
    }
}