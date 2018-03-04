using System;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using Microsoft.AspNetCore.Identity;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class DatabaseWebServerFixture : IDisposable
    {
        public DatabaseWebServerFixture()
        {

        }

        public UserManager<User> UserManager { get; private set; }

        public DatabasesFixture DatabasesFixture { get; private set; }

        public WebServer Server { get; private set; }

        public SiteTester SiteTester { get; private set; }

        public void Dispose()
        {
            SiteTester.Dispose();
            Server.Dispose();
            DatabasesFixture.Dispose();
        }

        public void Load()
        {
            DatabasesFixture = new DatabasesFixture();

            Server = new WebServer(DatabasesFixture.ConnectionStrings);
            var services = Server.Start();

            UserManager = (UserManager<User>)services.GetService(typeof(UserManager<User>));

            SiteTester = new SiteTester(new Uri(Server.Url));
        }
    }
}