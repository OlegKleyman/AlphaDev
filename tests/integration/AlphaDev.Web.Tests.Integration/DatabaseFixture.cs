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

        public DatabaseFixture()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("connectionstrings.json", false, true).Build();

            BlogContext = new BlogContext(
                string.Format(
                    CultureInfo.InvariantCulture,
                    configuration.GetConnectionString("integration"),
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