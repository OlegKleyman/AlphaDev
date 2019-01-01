using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Optional;

namespace AlphaDev.Web.Support
{
    public class Pager<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _collection;

        public Pager([NotNull] ICollection<T> collection, int startPage) : this(collection, startPage, false)
        {
            _collection = collection;
        }

        public Pager([NotNull] ICollection<T> collection, int startPage, bool hasAuxiliary)
        {
            _collection = collection;
            AuxiliaryPage = hasAuxiliary && collection.Count > 1 ? (collection.Count + startPage - 1).Some() : Option.None<int>();
            Pages = AuxiliaryPage.Map(x => collection.SkipLast(1)).WithException(collection).ValueOr(x => x)
                .Select((_, index) => index + startPage).ToArray();
        }

        public int[] Pages { get; }
        public Option<int> AuxiliaryPage { get; }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
