using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NSubstitute;
using Xunit;

namespace AlphaDev.Core.Data.Sql.Tests.Unit.Contexts
{
    public class InformationContextTests
    {
        [NotNull]
        private static MockInformationContext GetInformationContext()
        {
            return new MockInformationContext();
        }

        [NotNull]
        private static MockInformationContext GetInformationContext(Configurer configurer)
        {
            return new MockInformationContext(configurer);
        }

        [Fact]
        public void OnConfiguringShouldConfigureDbContextOptionsBuilder()
        {
            var configurer = Substitute.For<Configurer>();
            var context = GetInformationContext(configurer);
            var builder = new DbContextOptionsBuilder();
            context.OnConfiguringProxy(builder);
            configurer.Received(1).Configure(builder);
        }

        [Fact]
        public void OnModelCreatingShouldConfigureModelWithTheCorrectAboutConfiguration()
        {
            var context = GetInformationContext();

            var modelBuilder = new ModelBuilder(new ConventionSet());
            context.OnModelCreatingProxy(modelBuilder);

            var informationMetaData = modelBuilder.Entity<About>().Metadata;
            new
            {
                PrimaryKeyName = informationMetaData.FindPrimaryKey().Properties[0].Name,
                PrimaryKeyType = informationMetaData.FindPrimaryKey().Properties[0].ClrType,
                ValueNullable = informationMetaData.FindProperty("Value").IsNullable
            }.Should().BeEquivalentTo(new
            {
                PrimaryKeyName = "Id",
                PrimaryKeyType = typeof(bool),
                ValueNullable = false
            });
        }

        [Fact]
        public void OnModelCreatingShouldConfigureModelWithTheCorrectContactConfiguration()
        {
            var context = GetInformationContext();

            var modelBuilder = new ModelBuilder(new ConventionSet());
            context.OnModelCreatingProxy(modelBuilder);

            var informationMetaData = modelBuilder.Entity<Contact>().Metadata;
            new
            {
                PrimaryKeyName = informationMetaData.FindPrimaryKey().Properties[0].Name,
                PrimaryKeyType = informationMetaData.FindPrimaryKey().Properties[0].ClrType,
                ValueNullable = informationMetaData.FindProperty("Value").IsNullable
            }.Should().BeEquivalentTo(new
            {
                PrimaryKeyName = "Id",
                PrimaryKeyType = typeof(bool),
                ValueNullable = false
            });
        }
    }
}