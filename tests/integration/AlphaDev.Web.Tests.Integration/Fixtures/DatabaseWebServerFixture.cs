using System;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class DatabaseWebServerFixture : IDisposable
    {
        private readonly IServiceScope _serviceScope;

        public DatabaseWebServerFixture()
        {
            DatabasesFixture = new DatabasesFixture();

            Server = new WebServer(DatabasesFixture.ConnectionStrings);
            var services = Server.Start();

            var b = (IApplicationBuilder)services.GetService(typeof(IApplicationBuilder));
            _serviceScope = b.ApplicationServices.CreateScope();
            UserManager = (UserManager<User>)_serviceScope.ServiceProvider.GetService(typeof(UserManager<User>));



            SiteTester = new SiteTester(new Uri(Server.Url));
        }

        public UserManager<User> UserManager { get; private set; }

        public DatabasesFixture DatabasesFixture { get; private set; }

        public WebServer Server { get; private set; }

        public SiteTester SiteTester { get; private set; }

        public void Dispose()
        {
            _serviceScope.Dispose();
            SiteTester.Dispose();
            Server.Dispose();
            DatabasesFixture.Dispose();
        }

        public void Load()
        {
            
        }
    }
}