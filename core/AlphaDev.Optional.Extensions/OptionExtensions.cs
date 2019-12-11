using Optional;

namespace AlphaDev.Optional.Extensions
{
    public static class OptionExtensions
    {
        public static Option<T> FilterNotNull<T>(this Option<T?> option) where T : class => option.NotNull()!;

        public static T GetValueOrException<T, TException>(this Option<T, TException> option) where TException : T
        {
            return option.ValueOr(x => x);
        }
    }
}