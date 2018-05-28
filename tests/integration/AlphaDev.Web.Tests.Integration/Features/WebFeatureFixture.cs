using System;
using AlphaDev.Web.Tests.Integration.Fixtures;
using JetBrains.Annotations;
using LightBDD.XUnit2;
using Xunit;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    public class WebFeatureFixture : FeatureFixture, IClassFixture<DatabaseWebServerFixture>, IDisposable
    {
        protected const string FullDateFormatRegularExpression = @"\w+,\s\w+\s\d{2},\s\d{4}";
        protected const string FullDateFormatString = "dddd, MMMM dd, yyyy";
        private readonly WebServer _server;

        protected WebFeatureFixture(ITestOutputHelper output,
            [NotNull] DatabaseWebServerFixture databaseWebServerFixture) :
            base(output)
        {
            _server = databaseWebServerFixture.Server;
            DatabasesFixture = databaseWebServerFixture.DatabasesFixture;

            SiteTester = databaseWebServerFixture.SiteTester;
            CommonSteps = new CommonSteps(SiteTester, DatabasesFixture);
            SiteTester.Driver.Manage().Cookies.DeleteAllCookies();
        }

        protected DatabasesFixture DatabasesFixture { get; }

        protected string Log => _server.Log;

        public SiteTester SiteTester { get; }
        public CommonSteps CommonSteps { get; }

        public void Dispose()
        {
            DatabasesFixture.DatabaseManager.ResetDatabases();
            _server.ClearLog();
        }
    }
}