using System;
using System.Collections.Generic;
using System.Linq;
using AlphaDev.Web.Support;
using JetBrains.Annotations;

namespace AlphaDev.Web.Extensions
{
    public static class Enumerable
    {
        [NotNull]
        public static Pager<T> ToPager<T>([NotNull] this IEnumerable<T> enumerable, PageDimensions dimensions,
            [NotNull] Support.PositiveInteger totalItemCount)
        {
            return new Pager<T>(enumerable.ToArray(), dimensions, totalItemCount);
        }
    }
}
