using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NSubstitute;
using Xunit;

namespace AlphaDev.Core.Data.Sql.Tests.Unit.Contexts
{
    public class BlogContextTests
    {
        [Fact]
        public void OnModelCreatingShouldConfigureModelWithTheCorrectConfiguration()
        {
            var context = GetBlogContext();

            var modelBuilder = new ModelBuilder(new ConventionSet());
            context.OnModelCreatingProxy(modelBuilder);

            var blogMetaData = modelBuilder.Entity<Blog>().Metadata;
            new
            {
                PrimaryKeyName = blogMetaData.FindPrimaryKey().Properties[0].Name,
                CreatedDefaultValue = blogMetaData.FindProperty("Created").FindAnnotation("Relational:DefaultValueSql")
                    .Value,
                CreatedGenerated = blogMetaData.FindProperty("Created").ValueGenerated,
                ModifiedNullable = blogMetaData.FindProperty("Modified").IsNullable,
                ContentNullable = blogMetaData.FindProperty("Content").IsNullable,
                TitleNullable = blogMetaData.FindProperty("Title").IsNullable
            }.Should().BeEquivalentTo(new
            {
                PrimaryKeyName = "Id",
                CreatedDefaultValue = "GETUTCDATE()",
                CreatedGenerated = ValueGenerated.OnAdd,
                ModifiedNullable = true,
                ContentNullable = false,
                TitleNullable = false
            });
        }

        [NotNull]
        private static MockBlogContext GetBlogContext()
        {
            return new MockBlogContext();
        }

        [NotNull]
        private static MockBlogContext GetBlogContext(Configurer configurer)
        {
            return new MockBlogContext(configurer);
        }

        [Fact]
        public void OnConfiguringShouldConfigureDbContextOptionsBuilder()
        {
            var configurer = Substitute.For<Configurer>();
            var context = GetBlogContext(configurer);
            var builder = new DbContextOptionsBuilder();
            context.OnConfiguringProxy(builder);
            configurer.Received(1).Configure(builder);
        }
    }
}