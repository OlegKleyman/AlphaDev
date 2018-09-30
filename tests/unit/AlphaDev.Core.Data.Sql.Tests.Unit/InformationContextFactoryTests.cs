using System;
using AlphaDev.Test.Core;
using AlphaDev.Test.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Core.Data.Sql.Tests.Unit
{
    public class InformationContextFactoryTests
    {
        [Fact]
        public void ConfigurerConstructorShouldInitializeInformationContextFactory()
        {
            Action constructor = () => new InformationContextFactory(default).EmptyCall();
            constructor.Should().NotThrow();
        }

        [Fact]
        public void DefaultConstructorShouldInitializeInformationContextFactory()
        {
            Action constructor = () => new InformationContextFactory().EmptyCall();
            constructor.Should().NotThrow();
        }
    }
}