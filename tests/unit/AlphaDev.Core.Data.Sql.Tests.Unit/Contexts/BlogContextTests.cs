using System.Data.SqlClient;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Sql.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Xunit;

namespace AlphaDev.Core.Data.Sql.Tests.Unit.Contexts
{
    public class BlogContextTests
    {
        [Fact]
        public void OnModelCreatingShouldConfigureModelWithTheCorrectConfiguration()
        {
            var context = new MockBlogContext();

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
    }
}