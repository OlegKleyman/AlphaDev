using System;
using AlphaDev.Test.Core;
using AlphaDev.Test.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace AlphaDev.Core.Data.Sql.Tests.Unit
{
    public class BlogContextFactoryTests
    {
        [Fact]
        public void ConstructorUsingConfigurerArgumentShouldNotThrow()
        {
            Action constructor = () => new BlogContextFactory(default).EmptyCall();
            constructor.Should().NotThrow();
        }

        [Fact]
        public void DefaultConstructorShouldNotThrow()
        {
            Action constructor = () => new BlogContextFactory().EmptyCall();
            constructor.Should().NotThrow();
        }
    }
}