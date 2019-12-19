using Optional;

namespace FluentAssertions.Optional.Extensions
{
    public static class OptionEither
    {
        public static OptionAssertions<T, TException> Should<T, TException>(this Option<T, TException> option) =>
            new OptionAssertions<T, TException>(option);
    }
}