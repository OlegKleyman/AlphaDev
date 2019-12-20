using Optional;

namespace FluentAssertions.Optional.Extensions
{
    public static class OptionMaybe
    {
        public static OptionAssertions<T> Should<T>(this Option<T> option) => new OptionAssertions<T>(option);
    }
}