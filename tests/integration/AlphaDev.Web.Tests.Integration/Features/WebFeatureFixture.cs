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

        protected WebFeatureFixture(ITestOutputHelper output, DatabaseWebServerFixture databaseWebServerFixture) :
            base(output)
        {
            _server = databaseWebServerFixture.Server;
            DatabaseFixture = databaseWebServerFixture.DatabaseFixture;
            DatabaseFixture.BlogContext.Database.Migrate();

            SiteTester = databaseWebServerFixture.SiteTester;
        }

        protected DatabaseFixture DatabaseFixture { get; }

        protected string Log => _server.Log;

        public SiteTester SiteTester { get; }

        public void Dispose()
        {
            DatabaseFixture.ResetDatabase();

            _server.ClearLog();
        }
    }
}