using System;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Sql.Contexts;
using AlphaDev.Core.Data.Sql.Support;
using AlphaDev.Test.Integration.Core.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class InformationContextDatabaseFixture : IDisposable
    {
        public InformationContextDatabaseFixture([NotNull] DatabaseConnectionFixture connection)
        {
            InformationContext = new InformationContext(new SqlConfigurer(connection.String));
            Initialize();

            connection.Reset += Initialize;
        }

        public InformationContext InformationContext { get; set; }

        [NotNull]
        public About DefaultAbout => new About
        {
            Value = "## test about"
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