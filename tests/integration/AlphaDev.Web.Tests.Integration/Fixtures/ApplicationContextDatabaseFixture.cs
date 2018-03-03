using System;
using AlphaDev.Core.Data.Account.Security.Sql.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class ApplicationContextDatabaseFixture : IDisposable
    {
        public ApplicationContextDatabaseFixture(string connectionString)
        {
            ConnectionString = connectionString;

            ApplicationContext = new ApplicationContext(ConnectionString);
        }

        public string ConnectionString { get; }
        public ApplicationContext ApplicationContext { get; }

        public void Dispose()
        {
            ApplicationContext.Dispose();
        }
    }
}