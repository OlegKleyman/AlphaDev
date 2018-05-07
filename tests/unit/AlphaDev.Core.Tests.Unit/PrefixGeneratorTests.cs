using System;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
    public class PrefixGeneratorTests
    {
        [Fact]
        public void GenerateShouldGeneratePrefixStartingWithLetterg()
        {
            GetPrefixGenerator().Generate().Should().StartWith("g");
        }

        [Fact]
        public void GenerateShouldGeneratePrefix()
        {
            GetPrefixGenerator().Generate().Should().MatchRegex("^g[a-f0-9]{32}$");
        }

        private PrefixGenerator GetPrefixGenerator()
        {
            return new PrefixGenerator();
        }
    }
}
