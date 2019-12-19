using FluentAssertions.Execution;
using Optional;
using Optional.Unsafe;

namespace FluentAssertions.Optional
{
    public class OptionAssertions<T>
    {
        public OptionAssertions(Option<T> subject) => Subject = subject;

        public Option<T> Subject { get; }

        public AndWhichConstraint<OptionAssertions<T>, T> HaveSome(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion.BecauseOf(because, becauseArgs)
                   .ForCondition(Subject.HasValue)
                   .FailWith("Option does not have a value.");
            return new AndWhichConstraint<OptionAssertions<T>, T>(this, Subject.ValueOrFailure());
        }

        public AndConstraint<OptionAssertions<T>> BeNone(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion.BecauseOf(because, becauseArgs)
                   .ForCondition(!Subject.HasValue)
                   .FailWith("Option has a value.");
            return new AndConstraint<OptionAssertions<T>>(this);
        }
    }
}