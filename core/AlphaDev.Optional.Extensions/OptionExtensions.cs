using System;
using System.Threading.Tasks;
using Optional;
using Optional.Unsafe;

namespace AlphaDev.Optional.Extensions
{
    public static class OptionExtensions
    {
        public static Option<T> FilterNotNull<T>(this Option<T?> option) where T : class => option.NotNull()!;

        public static T GetValueOrException<T, TException>(this Option<T, TException> option) where TException : T
        {
            return option.ValueOr(x => x);
        }

        public static async Task MatchSomeAsync<T, TException>(this Option<T, TException> option, Func<T, Task> some)
        {
            if (option.HasValue)
            {
                await some(option.ValueOrFailure());
            }
        }
    }
}