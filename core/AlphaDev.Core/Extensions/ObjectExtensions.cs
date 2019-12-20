using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace AlphaDev.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static T To<TTarget, T>(this TTarget target, [NotNull] Func<TTarget, T> map) => map(target);

        public static async Task<T>
            ToAsync<TTarget, T>(this ValueTask<TTarget> target, [NotNull] Func<TTarget, T> map) => map(await target);
    }
}