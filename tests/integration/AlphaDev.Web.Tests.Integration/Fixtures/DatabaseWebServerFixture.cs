using System;
using System.Linq;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using Microsoft.AspNetCore.Identity;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class DatabaseWebServerFixture : IDisposable
    {
        public DatabaseWebServerFixture()
        {
            DatabasesFixture = new DatabasesFixture();

            Server = new WebServer(
                DatabasesFixture.DatabaseManager.ConnectionStrings.ToDictionary(pair => pair.Key,
                    pair => pair.Value.connectionString));

            SiteTester = new SiteTester(new Uri(Server.Url));
        }

        public DatabasesFixture DatabasesFixture { get; }

        public WebServer Server { get; }

        public SiteTester SiteTester { get; }

        public void Dispose()
        {
            SiteTester.Dispose();
            Server.Dispose();
            DatabasesFixture.Dispose();
        }
    }
}