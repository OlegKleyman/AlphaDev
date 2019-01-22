using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Optional;
using AlphaDev.Web.Extensions;
using Optional.Collections;
using Enumerable = System.Linq.Enumerable;

namespace AlphaDev.Web.Support
{
    public class Pager<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _collection;

        public Pager([NotNull] ICollection<T> collection, PageDimensions dimensions, int total)
        {
            var totalPages = dimensions.Boundaries.GetTotalPages(total);
            var pagesToDisplay = Math.Min(dimensions.Boundaries.MaxTotal.Value, totalPages);

            CurrentPage = dimensions.Start;
            NextPages = Enumerable.Range(dimensions.Start.Value + 1, Math.Max(pagesToDisplay - 1, 0)).ToArray();
            AuxiliaryPage = totalPages > pagesToDisplay ? (NextPages.LastOrNone().ValueOr(CurrentPage.Value) + 1).Some() : Option.None<int>();

            var pagesRemaining = total - dimensions.Start.ToStartPosition(dimensions.Boundaries.Count).Value + 1;
            _collection = collection.Take(Math.Min(dimensions.Boundaries.Count.Value, pagesRemaining));
        }

        public int[] NextPages { get; }
        public Option<int> AuxiliaryPage { get; }
        public PositiveInteger CurrentPage { get; }

        public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
