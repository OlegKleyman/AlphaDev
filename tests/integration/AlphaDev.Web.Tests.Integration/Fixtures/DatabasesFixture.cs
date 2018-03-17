using System;

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