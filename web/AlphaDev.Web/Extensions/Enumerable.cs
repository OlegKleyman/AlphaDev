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
        public static Pager<T> ToPager<T>([NotNull] this IEnumerable<T> enumerable, int startPage,
            [NotNull] Func<ICollection<T>, bool> getHasAuxiliary)
        {
            var collection = enumerable.ToArray();
            return new Pager<T>(collection, startPage, getHasAuxiliary(collection));
        }
    }
}
