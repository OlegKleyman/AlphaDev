using Optional;

namespace AlphaDev.Optional.Extensions
{
    public static class OptionExtensions
    {
        public static Option<T> FilterNotNull<T>(this Option<T?> option) where T : class
        {
            return option.NotNull()!;
        }
    }
}
