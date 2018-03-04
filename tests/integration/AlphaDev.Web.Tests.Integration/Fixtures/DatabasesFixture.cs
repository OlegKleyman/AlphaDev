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
        private const string ConnectionStringTemplate =
            @"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=True;MultipleActiveResultSets=true;Database=";

        private readonly Dictionary<string, (string databaseName, string connectionString)> _databases =
            GetDatabases();

        public DatabasesFixture()
        {
            ConnectionStrings = _databases.ToDictionary(pair => pair.Key, pair => pair.Value.connectionString);

            ResetDatabase();

            BlogContextDatabaseFixture = new BlogContextDatabaseFixture(_databases["default"].connectionString);
            ApplicationContextDatabaseFixture =
                new ApplicationContextDatabaseFixture(_databases["defaultSecurity"].connectionString);
        }

        public Dictionary<string, string> ConnectionStrings { get; }
        public BlogContextDatabaseFixture BlogContextDatabaseFixture { get; }
        public ApplicationContextDatabaseFixture ApplicationContextDatabaseFixture { get; }

        public void Dispose()
        {
            ResetDatabase();
            BlogContextDatabaseFixture.Dispose();
            ApplicationContextDatabaseFixture.Dispose();
        }

        private static Dictionary<string, (string, string)> GetDatabases()
        {
            var @default = Guid.NewGuid().ToString("N");
            var defaultSecurity = Guid.NewGuid().ToString("N");

            return new Dictionary<string, (string, string)>
            {
                ["default"] = (@default, ConnectionStringTemplate + @default),
                ["defaultSecurity"] = (defaultSecurity, ConnectionStringTemplate + defaultSecurity)
            };
        }

        public void ResetDatabase()
        {
            ApplicationContextDatabaseFixture?.ApplicationContext.DetachAll();
            BlogContextDatabaseFixture?.BlogContext.DetachAll();

            using (var connection = new SqlConnection(ConnectionStringTemplate + "master"))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;

                    connection.Open();
                    foreach (var database in _databases)
                    {
                        command.CommandText =
                            string.Format(CultureInfo.InvariantCulture, Assets.DropDatabase,
                                database.Value.databaseName);

                        command.ExecuteNonQuery();
                    }
                }
            }
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