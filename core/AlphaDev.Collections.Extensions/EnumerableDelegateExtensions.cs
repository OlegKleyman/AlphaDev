using System;
using System.Collections.Generic;
using System.Linq;

namespace AlphaDev.Collections.Extensions
{
    public static class EnumerableDelegateExtensions
    {
        // ReSharper disable once CoVariantArrayConversion - all delegates are of the same type
        public static T? Combine<T>(this IEnumerable<T?> target) where T : Delegate =>
            (T?) Delegate.Combine(target.ToArray());
    }
}
