﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using JetBrains.Annotations;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class DatabaseManagerFixture : IDisposable
    {
        private const string ConnectionStringTemplate =
            @"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=True;MultipleActiveResultSets=true;Database=";

        private static readonly object SyncLock = new object();

        private bool _disposed;

        public DatabaseManagerFixture() => Connections = new Dictionary<string, DatabaseConnectionFixture>();

        public Dictionary<string, DatabaseConnectionFixture> Connections { get; }

        public void Dispose()
        {
            if (!_disposed)
            {
                foreach (var connection in Connections)
                {
                    connection.Value.Dispose();
                }

                _disposed = true;
            }
        }

        public DatabaseConnectionFixture Get([NotNull] string key)
        {
            ThrowIfDisposed();

            if (!Connections.TryGetValue(key, out var targetConnection))
            {
                lock (SyncLock)
                {
                    var databaseName = Guid.NewGuid().ToString("N");

                    using (var connection = new SqlConnection($"{ConnectionStringTemplate}master"))
                    {
                        using (var command = new SqlCommand())
                        {
                            command.CommandTimeout = 120;
                            command.Connection = connection;
                            command.CommandText = $"CREATE DATABASE [{databaseName}]";

                            connection.Open();

                            command.ExecuteNonQuery();
                        }
                    }

                    targetConnection = new DatabaseConnectionFixture(databaseName);
                }

                Connections.Add(key, targetConnection);
            }

            return targetConnection;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(GetType().FullName);
        }

        public void ResetDatabases()
        {
            ThrowIfDisposed();

            foreach (var connection in Connections)
            {
                connection.Value.ResetDatabase();
            }
        }
    }
}