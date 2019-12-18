using System;
using System.Threading.Tasks;
using Optional;
using Optional.Async;
using Optional.Unsafe;

namespace AlphaDev.Optional.Extensions
{
    public static class OptionTaskExtensions
    {
        public static async Task MatchSomeAsync<T, TException>(this Task<Option<T, TException>> optionTask, Func<T, Task> some)
        {
            if (await optionTask is { HasValue: true } option)
            {
                await some(option.ValueOrFailure());
            }
        }

        public static async Task MatchSomeAsync<T, TException>(this Task<Option<T, TException>> optionTask, Action<T> some)
        {
            if (await optionTask is { HasValue: true } option)
            {
                some(option.ValueOrFailure());
            }
        }

        public static async Task<T> GetValueOrExceptionAsync<T, TException>(this Task<Option<T, TException>> option) where TException : T
        {
            return (await option).ValueOr(x => x);
        }

        public static async Task<Option<T>> SomeNotNullAsync<T>(this ValueTask<T> task) => (await task).SomeNotNull();

        public static async Task<Option<T, TException>> SomeNotNullAsync<T, TException>(this ValueTask<T> task,
            Func<TException> exception) => (await task).SomeNotNull(exception);

        public static async Task<T> ValueOrAsync<T, TException>(this Task<Option<T, TException>> option,
            Func<TException, T> exception) => (await option).ValueOr(exception);
    }
}
