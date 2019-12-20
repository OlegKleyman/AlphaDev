using AlphaDev.Optional.Extensions.Unsafe;
using FluentAssertions.Execution;
using Optional;
using Optional.Unsafe;

namespace FluentAssertions.Optional
{
    public class OptionAssertions<T, TException>
    {
        public OptionAssertions(Option<T, TException> subject) => Subject = subject;

        public Option<T, TException> Subject { get; }

        public AndWhichConstraint<OptionAssertions<T, TException>, T> HaveSome(string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion.BecauseOf(because, becauseArgs)
                   .ForCondition(Subject.HasValue)
                   .FailWith("Option does not have a value.");
            return new AndWhichConstraint<OptionAssertions<T, TException>, T>(this, Subject.ValueOrFailure());
        }

        public AndWhichConstraint<OptionAssertions<T, TException>, TException> BeNone(string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion.BecauseOf(because, becauseArgs)
                   .ForCondition(!Subject.HasValue)
                   .FailWith("Option has a value.");

            return new AndWhichConstraint<OptionAssertions<T, TException>, TException>(this,
                Subject.ExceptionOrFailure());
        }
    }
}