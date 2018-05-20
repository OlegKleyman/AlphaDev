using FluentAssertions;
using Xunit;

namespace AlphaDev.Core.Tests.Unit
{
    public class PrefixGeneratorTests
    {
        private PrefixGenerator GetPrefixGenerator()
        {
            return new PrefixGenerator();
        }

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