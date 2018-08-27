using System;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Sql.Contexts;
using AlphaDev.Test.Integration.Core.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class InformationContextDatabaseFixture : IDisposable
    {
        private readonly DatabaseConnectionFixture _connection;

        public InformationContextDatabaseFixture([NotNull] DatabaseConnectionFixture connection)
        {
            _connection = connection;
            InformationContext = new InformationContext(connection.String);
            Initialize();

            connection.Reset += Initialize;
        }

        public InformationContext InformationContext { get; set; }

        [NotNull]
        public About DefaultAbout => new About
        {
            Value = "## test about",
            ChangedOn = new DateTime(2018, 8, 23)
        };

        private void Initialize()
        {
            InformationContext.DetachAll();
            InformationContext.Database.Migrate();
        }

        public void Dispose()
        {
        }
    }
}