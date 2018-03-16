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
    }
}