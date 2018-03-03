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

            Server = new WebServer(DatabasesFixture.ConnectionStrings);
            var services = Server.Start();
            var userManager = (UserManager<User>) services.GetService(typeof(UserManager<User>));
            var result = userManager.CreateAsync(new User {UserName = "something@something.com"}, "H3ll04321!").GetAwaiter().GetResult();
            if (result != IdentityResult.Success)
            {
                throw new InvalidOperationException(string.Join(Environment.NewLine,
                    result.Errors.Select(error => error.Description)));
            }

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