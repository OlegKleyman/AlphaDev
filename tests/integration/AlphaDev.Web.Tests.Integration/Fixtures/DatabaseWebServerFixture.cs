using System;
using System.Linq;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class DatabaseWebServerFixture : IDisposable
    {
        public DatabaseWebServerFixture()
        {
            DatabasesFixture = new DatabasesFixture();

            Server = new WebServer(
                DatabasesFixture.DatabaseManager.Connections.ToDictionary(pair => pair.Key,
                    pair => pair.Value.String));

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