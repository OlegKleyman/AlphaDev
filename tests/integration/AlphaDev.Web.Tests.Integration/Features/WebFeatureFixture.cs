using System;
using AlphaDev.Web.Tests.Integration.Fixtures;
using LightBDD.XUnit2;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    public class WebFeatureFixture : FeatureFixture, IClassFixture<DatabaseWebServerFixture>, IDisposable
    {
        protected const string FullDateFormatRegularExpression = @"\w+,\s\w+\s\d{2},\s\d{4}";
        protected const string FullDateFormatString = "dddd, MMMM dd, yyyy";
        private readonly WebServer _server;
        private static readonly object Syncroot = new object();

        protected WebFeatureFixture(ITestOutputHelper output, DatabaseWebServerFixture databaseWebServerFixture) :
            base(output)
        {
            lock (Syncroot)
            {
                databaseWebServerFixture.Load();
                _server = databaseWebServerFixture.Server;
                DatabasesFixture = databaseWebServerFixture.DatabasesFixture;
                DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Database.Migrate();
                DatabasesFixture.ApplicationContextDatabaseFixture.ApplicationContext.Database.Migrate();
                DatabasesFixture.SeedUser(databaseWebServerFixture.UserManager);

                SiteTester = databaseWebServerFixture.SiteTester;
                CommonSteps = new CommonSteps(SiteTester, DatabasesFixture);
            }
        }

        protected DatabasesFixture DatabasesFixture { get; }

        protected string Log => _server.Log;

        public SiteTester SiteTester { get; }
        public CommonSteps CommonSteps { get; }

        public void Dispose()
        {
            DatabasesFixture.ResetDatabase();
            _server.ClearLog();
        }
    }
}