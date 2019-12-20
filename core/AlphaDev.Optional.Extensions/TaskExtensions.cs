using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Optional;

namespace AlphaDev.Optional.Extensions
{
    public static class TaskExtensions
    {
        public static async Task<Option<IEnumerable<T>, TException>> SomeNotEmptyAsync<T, TException>(
            this Task<IEnumerable<T>> task, Func<TException> exceptionFactory)
        {
            return (await task).SomeWhen(enumerable => enumerable.Any(), exceptionFactory);
        }
    }
}