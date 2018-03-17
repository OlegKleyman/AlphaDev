using System.Data.SqlClient;
using AlphaDev.Core.Data.Account.Security.Sql.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AlphaDev.Core.Data.Account.Security.Sql.Tests.Unit.Contexts
{
    public class ApplicationContextTests
    {
        [Fact]
        public void ConstructorShouldSetConnectionString()
        {
            const string server = "testDb";
            const string database = "testDatabase";

            var context = new ApplicationContext($"Server={server};Database={database};");

            context.Database.GetDbConnection().Should().BeOfType<SqlConnection>().Subject.Should().BeEquivalentTo(
                new {Database = database, DataSource = server},
                options => options.Including(connection => connection.Database)
                    .Including(connection => connection.DataSource));
        }
    }
}