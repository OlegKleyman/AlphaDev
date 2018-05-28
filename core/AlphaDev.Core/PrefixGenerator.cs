using System;
using JetBrains.Annotations;

namespace AlphaDev.Core
{
    public class PrefixGenerator : IPrefixGenerator
    {
        [NotNull]
        public string Generate()
        {
            return "g" + Guid.NewGuid().ToString("N");
        }
    }
}