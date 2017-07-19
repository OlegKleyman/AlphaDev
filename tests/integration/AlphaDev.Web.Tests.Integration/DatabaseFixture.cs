using System.Data.SqlClient;

namespace AlphaDev.Web.Tests.Integration
{
    using System;
    using System.Globalization;
    using System.IO;

    using AlphaDev.Core.Data.Sql.Contexts;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class DatabaseFixture: IDisposable
    {
        public BlogContext BlogContext { get; }
        public string ConnectionString { get; }

        public DatabaseFixture()
        {
            ConnectionString =
                $@"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=True;MultipleActiveResultSets=true;Database={Guid.NewGuid()};";
            
            BlogContext = new BlogContext(
                string.Format(
                    CultureInfo.InvariantCulture,
                    ConnectionString,
                    Directory.GetCurrentDirectory()));

            BlogContext.Database.EnsureDeleted();
            BlogContext.Database.Migrate();
        }

        public void Dispose()
        {
            BlogContext.Database.EnsureDeleted();
            BlogContext.Dispose();
        }
    }
}