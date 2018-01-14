using System;
using System.Linq;
using AlphaDev.Test.Integration.Core.Extensions;
using AlphaDev.Web.Tests.Integration.Fixtures;
using LightBDD.XUnit2;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    public class WebFeatureFixture : FeatureFixture, IClassFixture<DatabaseWebServerFixture>, IDisposable
    {
        private readonly WebServer _server;
        protected DatabaseFixture DatabaseFixture { get; }

        protected string Log => _server.Log;

        protected WebFeatureFixture(ITestOutputHelper output, DatabaseWebServerFixture databaseWebServerFixture) : base(output)
        {
            _server = databaseWebServerFixture.Server;
            DatabaseFixture = databaseWebServerFixture.DatabaseFixture;
            DatabaseFixture.BlogContext.Database.Migrate();

            SiteTester = databaseWebServerFixture.SiteTester;
        }

        public SiteTester SiteTester { get; }

        public void Dispose()
        {
            DatabaseFixture.ResetDatabase();

            _server.ClearLog();
        }
    }
}