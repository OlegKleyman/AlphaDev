using System.Data.SqlClient;
using AlphaDev.Core.Data.Sql.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
    }
}