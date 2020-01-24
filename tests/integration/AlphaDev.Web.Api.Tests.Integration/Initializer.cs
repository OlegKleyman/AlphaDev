using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AlphaDev.Core.Data.Account.Security.Sql.Contexts;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Sql.Support;
using AlphaDev.Core.Data.Support;
using AspNetCoreTestServer.Core;
using BoDi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TechTalk.SpecFlow;

namespace AlphaDev.Web.Api.Tests.Integration
{
    [Binding]
    public class Initializer
    {
        private readonly IObjectContainer _container;
        private static readonly object SyncLock;

        static Initializer() => SyncLock = new object();

        public Initializer(IObjectContainer container) => _container = container;

        [BeforeScenario(Order = int.MinValue)]
        public void CreateContainerBuilder()
        {
            lock (SyncLock)
            {
                SetupWebServer();
                SetupBlogDatabase();
            }
        }

        private void SetupWebServer()
        {
            _container.RegisterTypeAs<PortResolver, IPortResolver>();
            _container.RegisterFactoryAs(container => (Func<IWebHostBuilder>) WebHost.CreateDefaultBuilder);
            _container.RegisterTypeAs<KestrelWebServer, IWebServer>();
        }

        private void SetupBlogDatabase()
        {
            var defaultBuilder = new SqlConnectionStringBuilder
            {
                IntegratedSecurity = true,
                DataSource = @"(LocalDB)\MSSQLLocalDB",
                MultipleActiveResultSets = true,
                InitialCatalog = $"blog_{Guid.NewGuid():N}",
                Pooling = false,
                ConnectTimeout = 120
            };

            var securityBuilder = new SqlConnectionStringBuilder
            {
                IntegratedSecurity = true,
                DataSource = @"(LocalDB)\MSSQLLocalDB",
                MultipleActiveResultSets = true,
                InitialCatalog = $"security_{Guid.NewGuid():N}",
                Pooling = false,
                ConnectTimeout = 120
            };

            _container.RegisterFactoryAs<BlogContext>(container =>
                new Core.Data.Sql.Contexts.BlogContext(new SqlConfigurer(defaultBuilder.ToString())));
            _container.RegisterFactoryAs<IdentityDbContext<User>>(container =>
                new ApplicationContext(new SqlConfigurer(securityBuilder.ToString())));
        }

        [AfterScenario(Order = int.MaxValue)]
        public async Task Cleanup()
        {
            BlogContext blogContext;
            IdentityDbContext<User> securityContext;

            lock (SyncLock)
            {
                blogContext = _container.Resolve<BlogContext>();
                securityContext = _container.Resolve<IdentityDbContext<User>>();
            }

            await blogContext.Database.EnsureDeletedAsync();
            await securityContext.Database.EnsureDeletedAsync();
        }
    }
}
