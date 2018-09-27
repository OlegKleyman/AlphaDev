using System;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class DatabasesFixture : IDisposable
    {
        public DatabasesFixture()
        {
            DatabaseManager = new DatabaseManagerFixture();

            var connection = DatabaseManager.Get("default");
            BlogContextDatabaseFixture = new BlogContextDatabaseFixture(connection);
            ApplicationContextDatabaseFixture =
                new ApplicationContextDatabaseFixture(DatabaseManager.Get("defaultSecurity"));
            InformationContextDatabaseFixture = new InformationContextDatabaseFixture(connection);
        }

        public BlogContextDatabaseFixture BlogContextDatabaseFixture { get; }
        public ApplicationContextDatabaseFixture ApplicationContextDatabaseFixture { get; }
        public DatabaseManagerFixture DatabaseManager { get; }
        public InformationContextDatabaseFixture InformationContextDatabaseFixture { get; }

        public void Dispose()
        {
            DatabaseManager.Dispose();
            BlogContextDatabaseFixture.Dispose();
            ApplicationContextDatabaseFixture.Dispose();
        }
    }
}