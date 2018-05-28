using System;
using AlphaDev.Core.Data.Account.Security.Sql.Contexts;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Test.Integration.Core.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class ApplicationContextDatabaseFixture : IDisposable
    {
        private readonly DatabaseConnectionFixture _connection;

        public ApplicationContextDatabaseFixture([NotNull] DatabaseConnectionFixture connection)
        {
            _connection = connection;
            ApplicationContext = new ApplicationContext(connection.String);
            Initialize();

            connection.Reset += Initialize;
        }

        public ApplicationContext ApplicationContext { get; }

        public void Dispose()
        {
            ApplicationContext.Dispose();
            _connection.Dispose();
        }

        private void Initialize()
        {
            ApplicationContext.DetachAll();
            ApplicationContext.Database.Migrate();
            SeedUser();
        }

        private void SeedUser()
        {
            var user = new User
            {
                UserName = "something@something.com",
                AccessFailedCount = default,
                ConcurrencyStamp = "dda9cebb-c520-494c-b365-4823f1c0b938",
                Email = "something@something.com",
                EmailConfirmed = default,
                LockoutEnabled = true,
                Id = Guid.NewGuid().ToString("D"),
                LockoutEnd = default,
                NormalizedEmail = "SOMETHING@SOMETHING.COM",
                NormalizedUserName = "SOMETHING@SOMETHING.COM",
                PasswordHash = "AQAAAAEAACcQAAAAEC8J5oSDteONWmC4w/wTQj4ylVY+UnNjYWKgof2+VdMacVM2TbtW76DBx+Arx/1tzg==",
                PhoneNumber = default,
                PhoneNumberConfirmed = default,
                SecurityStamp = "8c0df882-fdeb-4e71-a588-77ee702facce",
                TwoFactorEnabled = default
            };

            ApplicationContext.Users.Add(user);
            ApplicationContext.SaveChanges();
        }
    }
}