using System;
using AlphaDev.Core.Data.Sql.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Xunit;

namespace AlphaDev.Core.Data.Sql.Tests.Unit.Support
{
    public class SqlConfigurerTests
    {
        [NotNull]
        private static SqlConfigurer GetSqlConfigurer(string connectionString)
        {
            return new SqlConfigurer(connectionString);
        }

        [Fact]
        public void ConfigureShouldConfigureSqlOptions()
        {
            const string connectionString = "Server=testServer;Database=testDb;";
            var configurer = GetSqlConfigurer(connectionString);
            var builder = new DbContextOptionsBuilder();
            configurer.Configure(builder);

            builder.Options.GetExtension<SqlServerOptionsExtension>().ConnectionString
                .Should().BeEquivalentTo(connectionString);
        }

        [Fact]
        public void ConstructorShouldInitializeSqlConfigurer()
        {
            Action constructor = () => new SqlConfigurer(default);
            constructor.Should().NotThrow();
        }
    }
}