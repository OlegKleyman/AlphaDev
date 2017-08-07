using System.Data.SqlClient;
using System.Reflection;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Sql.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration.Memory;
using Xunit;

namespace AlphaDev.Core.Data.Sql.Tests.Unit.Contexts
{
    public class BlogContextTests
    {
        [Fact]
        public void ConstructorShouldSetConnectionString()
        {
            const string server = "testDb";
            const string database = "testDatabase";

            var context = new BlogContext($"Server={server};Database={database};");

            context.Database.GetDbConnection().Should().BeOfType<SqlConnection>().Subject.ShouldBeEquivalentTo(
                new {Database = database, DataSource = server},
                options => options.Including(connection => connection.Database)
                    .Including(connection => connection.DataSource));
        }

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
            }.ShouldBeEquivalentTo(new
            {
                PrimaryKeyName = "Id",
                CreatedDefaultValue = "GETUTCDATE()",
                CreatedGenerated = ValueGenerated.OnAdd,
                ModifiedNullable = true,
                ContentNullable = false,
                TitleNullable = false
            });
        }

        [Fact]
        public void OnConfiguringShouldConfigureToUseSqlServerWithAssignedConnectionString()
        {
            const string connectionString = "Server=testServer;Database=testDb;";
            var context = new MockBlogContext(connectionString);

            var builder = new DbContextOptionsBuilder();
            context.OnConfiguringProxy(builder);

            builder.Options.GetExtension<SqlServerOptionsExtension>().ConnectionString
                .ShouldBeEquivalentTo(connectionString);
        }
    }
}