﻿using System.Collections.Generic;
using System.Linq;
using AlphaDev.Core;
using AlphaDev.Web.Core.Support;
using JetBrains.Annotations;

namespace AlphaDev.Web.Core.Extensions
{
    public static class Enumerable
    {
        [NotNull]
        public static Pager<T> ToPager<T>([NotNull] this IEnumerable<T> enumerable, PageDimensions dimensions,
            [NotNull] PositiveInteger totalItemCount) => new Pager<T>(enumerable.ToArray(), dimensions, totalItemCount);
    }
}