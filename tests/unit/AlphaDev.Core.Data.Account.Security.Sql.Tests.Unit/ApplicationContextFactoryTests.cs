using System;
using AlphaDev.Test.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Core.Data.Account.Security.Sql.Tests.Unit
{
    public class ApplicationContextFactoryTests
    {
        [Fact]
        public void ConstructorUsingConfigurerArgumentShouldNotThrow()
        {
            Action constructor = () => new ApplicationContextFactory(default).EmptyCall();
            constructor.Should().NotThrow();
        }

        [Fact]
        public void DefaultConstructorShouldNotThrow()
        {
            Action constructor = () => new ApplicationContextFactory().EmptyCall();
            constructor.Should().NotThrow();
        }
    }
}