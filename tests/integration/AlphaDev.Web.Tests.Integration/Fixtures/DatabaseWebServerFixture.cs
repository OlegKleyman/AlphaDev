﻿using System;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using Microsoft.AspNetCore.Identity;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class DatabaseWebServerFixture : IDisposable
    {
        public DatabaseWebServerFixture()
        {
            DatabasesFixture = new DatabasesFixture();

            Server = new WebServer(DatabasesFixture.ConnectionStrings);

            SiteTester = new SiteTester(new Uri(Server.Url));
        }

        public DatabasesFixture DatabasesFixture { get; }

        public WebServer Server { get; }

        public SiteTester SiteTester { get; }

        public void Dispose()
        {
            SiteTester.Dispose();
            Server.Dispose();
            DatabasesFixture.Dispose();
        }
    }
}