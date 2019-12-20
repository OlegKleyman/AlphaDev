using System;
using System.Threading.Tasks;
using Optional;

namespace AlphaDev.Optional.Extensions
{
    public static class ValueTaskExtensions
    {
        public static async Task<Option<T>> SomeNotNullAsync<T>(this ValueTask<T> task) => (await task).SomeNotNull();

        public static async Task<Option<T, TException>> SomeNotNullAsync<T, TException>(this ValueTask<T> task,
            Func<TException> exception) => (await task).SomeNotNull(exception);
    }
}