using System;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class DatabaseWebServerFixture : IDisposable
    {
        public DatabaseWebServerFixture()
        {
            DatabaseFixture = new DatabaseFixture();

            Server = new WebServer(DatabaseFixture.ConnectionString);

            SiteTester = new SiteTester(new Uri(Server.Url));
        }

        public WebServer Server { get; }

        public DatabaseFixture DatabaseFixture { get; }

        public SiteTester SiteTester { get; }

        public void Dispose()
        {
            SiteTester.Dispose();
            Server.Dispose();
            DatabaseFixture.Dispose();
        }
    }
}