using System;
using JetBrains.Annotations;

namespace AlphaDev.Test.Core.Support
{
    public class Switch<T>
    {
        public bool On { get; set; }

        public Switch([NotNull] Func<Switch<T>, T> func) => Target = func(this);

        public T Target { get; }
    }
}