using System;
using JetBrains.Annotations;

namespace AlphaDev.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static T Map<TTarget, T>(this TTarget target, [NotNull] Func<TTarget, T> map)
        {
            return map(target);
        }
    }
}