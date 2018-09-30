using System;
using AlphaDev.Core.Data.Sql.Contexts;
using AlphaDev.Core.Data.Support;
using AlphaDev.Test.Core;
using AlphaDev.Test.Core.Extensions;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using NSubstitute;
using Xunit;

namespace AlphaDev.Core.Data.Sql.Tests.Unit.ContextBuilders
{
    public class InformationContextBuilderTests
    {
        [NotNull]
        private ContextFactories.InformationContextFactory GetInformationContextFactory(
            [NotNull] IDesignTimeDbContextFactory<InformationContext> factory)
        {
            return new ContextFactories.InformationContextFactory(factory);
        }

        [Fact]
        public void ConstructorShouldInitializeNewObject()
        {
            Action constructor = () =>
                new ContextFactories.InformationContextFactory(
                    Substitute.For<AlphaContextFactory<InformationContext>>(Substitute.For<Configurer>())).EmptyCall();
            constructor.Should().NotThrow();
        }

        [Fact]
        public void CreateShouldCreateNewInformationContext()
        {
            var factoryMock =
                Substitute
                    .For<IDesignTimeDbContextFactory<InformationContext>>();
            var context = Substitute.For<InformationContext>((Configurer) default);
            factoryMock.CreateDbContext(Arg.Any<string[]>()).Returns(context);
            var factory = GetInformationContextFactory(factoryMock);

            factory.Create().Should().NotBeNull();
        }
    }
}