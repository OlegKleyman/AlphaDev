using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Test.Integration.Core.Extensions;
using Microsoft.AspNetCore.Identity;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class DatabasesFixture : IDisposable
    {
        public DatabasesFixture()
        {
            DatabaseManager = new DatabaseManagerFixture();
            DatabaseManager.OnReset += () =>
            {
                BlogContextDatabaseFixture.BlogContext.DetachAll();
                ApplicationContextDatabaseFixture.ApplicationContext.DetachAll();
            };

            BlogContextDatabaseFixture = new BlogContextDatabaseFixture(DatabaseManager.Get("default"));
            ApplicationContextDatabaseFixture =
                new ApplicationContextDatabaseFixture(DatabaseManager.Get("defaultSecurity"));
        }
        
        public BlogContextDatabaseFixture BlogContextDatabaseFixture { get; }
        public ApplicationContextDatabaseFixture ApplicationContextDatabaseFixture { get; }
        public DatabaseManagerFixture DatabaseManager { get; }

        public void Dispose()
        {
            DatabaseManager.Dispose();
            BlogContextDatabaseFixture.Dispose();
            ApplicationContextDatabaseFixture.Dispose();
        }

        public void SeedUser()
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

            ApplicationContextDatabaseFixture.ApplicationContext.Users.Add(user);
            ApplicationContextDatabaseFixture.ApplicationContext.SaveChanges();
        }
    }
}