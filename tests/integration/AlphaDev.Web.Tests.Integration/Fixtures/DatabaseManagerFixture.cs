using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class DatabaseManagerFixture
    {
        string ConnectionStringTemplate =
            @"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=True;MultipleActiveResultSets=true;Database=";

        private bool _disposed;

        public DatabaseManagerFixture()
        {
            ConnectionStrings = new Dictionary<string, (string, string)>();
        }

        public Dictionary<string, (string databaseName, string connectionString)> ConnectionStrings { get; }
        public event Action OnReset;

        public string Get(string key)
        {
            ThrowIfDisposed();

            if (!ConnectionStrings.TryGetValue(key, out var connectionString))
            {
                var databaseName = Guid.NewGuid().ToString("N");

                using (var connection = new SqlConnection($"{ConnectionStringTemplate}master"))
                {
                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = $"CREATE DATABASE [{databaseName}]";

                        connection.Open();

                        command.ExecuteNonQuery();
                    }
                }

                var targetDatabaseConnectionString = ConnectionStringTemplate + databaseName;

                ConnectionStrings.Add(key, (databaseName, targetDatabaseConnectionString));
                connectionString.connectionString = targetDatabaseConnectionString;
            }

            return connectionString.connectionString;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        public void ResetDatabases()
        {
            ThrowIfDisposed();

            using (var connection = new SqlConnection(ConnectionStringTemplate + "master"))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;

                    connection.Open();
                    command.CommandText = string.Join(Environment.NewLine,
                        ConnectionStrings.Select(pair => string.Format(CultureInfo.InvariantCulture,
                            Assets.DropDatabase, pair.Value.databaseName)));

                    command.ExecuteNonQuery();
                }
            }

            if (OnReset != null)
            {
                var cachedOnReset = OnReset;

                cachedOnReset();
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                ResetDatabases();

                _disposed = true;
            }
        }
    }
}