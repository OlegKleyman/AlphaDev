using System;
using AlphaDev.Core.Data.Sql.Support;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Xunit;

namespace AlphaDev.Core.Data.Sql.Tests.Unit.Support
{
    public class Sql2008ConfigurerTests
    {
        [NotNull]
        private static SqlConfigurer GetSqlConfigurer(string connectionString) => new SqlConfigurer(connectionString);

        [Fact]
        public void ConfigureShouldConfigureSqlOptions()
        {
            const string connectionString = "Server=testServer;Database=testDb;";
            var configurer = GetSqlConfigurer(connectionString);
            var builder = new DbContextOptionsBuilder();
            configurer.Configure(builder);
            builder.Options.GetExtension<SqlServerOptionsExtension>()
                   .Should()
                   .BeEquivalentTo(new
                           { ConnectionString = connectionString, LogFragment = "RowNumberPaging " },
                       options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void ConstructorShouldInitializeSqlConfigurer()
        {
            Action constructor = () => new SqlConfigurer(default);
            constructor.Should().NotThrow();
        }
    }
}