using System;
using JetBrains.Annotations;

namespace AlphaDev.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static T To<TTarget, T>(this TTarget target, [NotNull] Func<TTarget, T> map) => map(target);
    }
}