using System;
using System.Data.SqlClient;
using System.Globalization;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class DatabaseConnectionFixture : IDisposable
    {
        private const string ConnectionStringTemplate =
            @"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=True;MultipleActiveResultSets=true;Database=";

        private readonly string _database;

        private bool _disposed;

        public DatabaseConnectionFixture(string database)
        {
            _database = database;

            var builder = new SqlConnectionStringBuilder
            {
                IntegratedSecurity = true,
                DataSource = @"(LocalDB)\MSSQLLocalDB",
                MultipleActiveResultSets = true,
                InitialCatalog = database,
                Pooling = false,
                ConnectTimeout = 120
            };

            String = builder.ToString();
        }

        public string String { get; }

        public void Dispose()
        {
            if (!_disposed)
            {
                DeleteDatabases();

                _disposed = true;
            }
        }

        public event Action Reset;

        protected virtual void OnReset()
        {
            if (Reset != null)
            {
                var cachedDisposed = Reset;

                cachedDisposed();
            }
        }

        public void ResetDatabase()
        {
            DeleteDatabases();

            OnReset();
        }

        private void DeleteDatabases()
        {
            ThrowIfDisposed();

            using (var connection = new SqlConnection(ConnectionStringTemplate + "master"))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;

                    connection.Open();
                    command.CommandText = string.Format(CultureInfo.InvariantCulture, Assets.DropDatabase, _database);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(GetType().FullName);
        }
    }
}