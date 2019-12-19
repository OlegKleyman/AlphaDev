﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AlphaDev.Core;
using JetBrains.Annotations;
using Optional;
using Optional.Collections;

namespace AlphaDev.Web.Core.Support
{
    public class Pager<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _collection;

        public Pager([NotNull] ICollection<T> collection, PageDimensions dimensions, [NotNull] PositiveInteger total)
        {
            var totalPages = dimensions.Boundaries.GetTotalPages(total.Value);
            var pagesToDisplay = Math.Min(dimensions.Boundaries.MaxTotal.Value, totalPages);

            PreviousPages = Enumerable
                            .Range(dimensions.Start.Value - dimensions.Boundaries.MaxTotal.Value,
                                dimensions.Boundaries.MaxTotal.Value)
                            .Where(x => x > 0)
                            .ToArray();
            CurrentPage = dimensions.Start;
            NextPages = Enumerable.Range(dimensions.Start.Value + 1, Math.Max(pagesToDisplay - 1, 0)).ToArray();
            AuxiliaryPage = totalPages > pagesToDisplay
                ? NextPages.LastOrNone().Or(() => CurrentPage.Value).Map(i => i + 1)
                : Option.None<int>();

            _collection = collection.Take(dimensions.Boundaries.Count.Value);
        }

        public int[] PreviousPages { get; set; }

        public int[] NextPages { get; }

        public Option<int> AuxiliaryPage { get; }

        public PositiveInteger CurrentPage { get; }

        public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}