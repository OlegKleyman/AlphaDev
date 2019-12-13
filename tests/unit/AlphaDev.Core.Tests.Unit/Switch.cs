using System;
using JetBrains.Annotations;

namespace AlphaDev.Core.Tests.Unit
{
    public class Switch<T>
    {
        public bool On { get; set; }

        public Switch([NotNull] Func<Switch<T>, T> func) => Target = func(this);

        public T Target { get; }
    }
}