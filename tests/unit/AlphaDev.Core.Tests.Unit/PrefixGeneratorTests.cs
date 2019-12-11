using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
    public class PrefixGeneratorTests
    {
        [NotNull]
        private PrefixGenerator GetPrefixGenerator() => new PrefixGenerator();

        [Fact]
        public void GenerateShouldGeneratePrefix()
        {
            GetPrefixGenerator().Generate().Should().MatchRegex("^g[a-f0-9]{32}$");
        }

        [Fact]
        public void GenerateShouldGeneratePrefixStartingWithLetterg()
        {
            GetPrefixGenerator().Generate().Should().StartWith("g");
        }
    }
}