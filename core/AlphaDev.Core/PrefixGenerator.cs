using System;

namespace AlphaDev.Core
{
    public class PrefixGenerator : IPrefixGenerator
    {
        public string Generate() => "g" + Guid.NewGuid().ToString("N");
    }
}