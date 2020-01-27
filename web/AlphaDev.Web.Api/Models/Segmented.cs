using System;
using JetBrains.Annotations;

namespace AlphaDev.Web.Api.Models
{
    public class Segmented<T>
    {
        private T[]? _values;

        [NotNull]
        public T[] Values
        {
            get => _values ?? throw new InvalidOperationException();
            set => _values = value;
        }

        public int Total { get; set; }
    }
}
