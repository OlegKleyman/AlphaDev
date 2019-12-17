using System;
using System.Threading.Tasks;
using Optional;
using Optional.Unsafe;

namespace AlphaDev.Optional.Extensions
{
    public static class OptionTaskExtension
    {
        public static async Task MatchSomeAsync<T, TException>(this Task<Option<T, TException>> optionTask, Func<T, Task> some)
        {
            if (await optionTask is { HasValue: true } option)
            {
                await some(option.ValueOrFailure());
            }
        }

        public static async Task<T> GetValueOrExceptionAsync<T, TException>(this Task<Option<T, TException>> option) where TException : T
        {
            return (await option).ValueOr(x => x);
        }

        public static TResult To<T, TResult>(this T target, Func<T, TResult> result) => result(target);
    }
}
