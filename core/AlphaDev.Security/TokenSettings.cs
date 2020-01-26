using System;
using JetBrains.Annotations;

namespace AlphaDev.Security
{
    public class TokenSettings
    {
        public string? Issuer { get; set; }

        public string? Audience { get; set; }

        public string? Key { get; set; }

        public int SecondsValid { get; set; }

        public string? Algorithm { get; set; }
    }
}