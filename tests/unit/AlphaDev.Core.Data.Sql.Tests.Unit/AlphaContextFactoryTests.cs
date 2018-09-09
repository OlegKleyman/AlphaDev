using System;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Support;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;

namespace AlphaDev.Core.Data.Sql.Tests.Unit
{
    public class AlphaContextFactoryTests
    {
        private static AlphaContextFactory<T> GetAlphaContextFactory<T>(Configurer configurer) where T : DbContext
        {
            return Substitute.For<AlphaContextFactory<T>>(configurer);
        }

        public class TestContext2 : TestContext
        {
            private TestContext2(Configurer configurer) : base(configurer)
            {
            }
        }

        public class TestContext : AlphaContext
        {
            public TestContext(Configurer configurer) : base(configurer)
            {
                Configurer = configurer;
            }

            public Configurer Configurer { get; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                throw new NotImplementedException();
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void ConstructorShouldCreateInstance()
        {
            Action constructor = () =>
                Substitute.For<AlphaContextFactory<AlphaContext>>(default(IConfigurationRoot));
            constructor.Should().NotThrow();
        }

        [Fact]
        public void CreateDbContextShouldCreateContextWithConfigurerFromConstructor()
        {
            var configurer = Substitute.For<Configurer>();
            var factory = GetAlphaContextFactory<TestContext>(configurer);
            factory.CreateDbContext().Configurer.Should().Be(configurer);
        }

        [Fact]
        public void CreateDbContextShouldCreateContextWithDefaultConnectionString()
        {
            var factory = GetAlphaContextFactory<TestContext>(null);
            var optionsBuilder = new DbContextOptionsBuilder();
            factory.CreateDbContext().Configurer.Configure(optionsBuilder);
            optionsBuilder.Options.FindExtension<SqlServerOptionsExtension>().ConnectionString.Should()
                .BeEquivalentTo(@"Data Source=(LocalDB)\v11.0;");
        }

        [Fact]
        public void CreateDbContextShouldThrowInvalidOperationExceptionExceptionWhenNotConcreteContext()
        {
            var factory = GetAlphaContextFactory<AlphaContext>(null);
            Action createDbContext = () => factory.CreateDbContext();
            createDbContext.Should().Throw<InvalidOperationException>().WithMessage(
                $"Unable to instantiate {typeof(AlphaContext).FullName} because it is abstract.");
        }

        [Fact]
        public void CreateDbContextShouldThrowMissingMethodExceptionWhenNoAcceptableConstructorIsFound()
        {
            var factory = GetAlphaContextFactory<TestContext2>(null);
            Action createDbContext = () => factory.CreateDbContext();
            createDbContext.Should().Throw<MissingMethodException>().WithMessage(
                $"Unable to find a public constructor with a single {typeof(Configurer).FullName} argument.");
        }
    }
}