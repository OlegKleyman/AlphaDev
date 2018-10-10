using System;
using AlphaDev.Core.Data.Sql.ContextFactories;
using AlphaDev.Test.Core.Extensions;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using NSubstitute;
using Xunit;

namespace AlphaDev.Core.Data.Sql.Tests.Unit.ContextBuilders
{
    public class ContextFactoryTests
    {
        [NotNull]
        private ContextFactory<DbContext> GetContextFactory(
            [NotNull] IDesignTimeDbContextFactory<DbContext> factory)
        {
            return new ContextFactory<DbContext>(factory);
        }

        [Fact]
        public void ConstructorShouldInitializeNewObject()
        {
            Action constructor = () =>
                new ContextFactory<DbContext>(
                    Substitute.For<IDesignTimeDbContextFactory<DbContext>>()).EmptyCall();
            constructor.Should().NotThrow();
        }

        [Fact]
        public void CreateShouldCreateNewDbContext()
        {
            var factoryMock =
                Substitute
                    .For<IDesignTimeDbContextFactory<DbContext>>();
            var context = Substitute.For<DbContext>();
            factoryMock.CreateDbContext(Arg.Any<string[]>()).Returns(context);
            var factory = GetContextFactory(factoryMock);

            factory.Create().Should().NotBeNull();
        }
    }
}