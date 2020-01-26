using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Extensions;
using AlphaDev.Web.Api.Tests.Integration.Support;
using AspNetCoreTestServer.Core;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Optional;
using Optional.Unsafe;
using TechTalk.SpecFlow;

namespace AlphaDev.Web.Api.Tests.Integration.ScenarioSetup
{
    [Binding]
    public class WebServer : Steps
    {
        public WebServer([NotNull] ScenarioContext scenarioContext, IWebServer server) => scenarioContext.Set(server);

        [BeforeScenario("web", Order = (int) ScenarioOrder.Web)]
        public async Task StartServer()
        {
            var configuration = new Dictionary<string, string>
            {
                ["connectionStrings:default"] = ScenarioContext.TryGetValue<BlogContext>(out var blogContext).SomeWhen(b => b).Map(b => blogContext.Database.GetDbConnection().ConnectionString).ValueOrDefault(),
                ["connectionStrings:security"] = ScenarioContext.TryGetValue<IdentityDbContext<User>>(out var identityContext).SomeWhen(b => b).Map(b => identityContext.Database.GetDbConnection().ConnectionString).ValueOrDefault(),
                ["Jwt:Issuer"] = "issuer",
                ["Jwt:Audience"] = "audience",
                ["Jwt:Key"] = "key12345578909877543210",
                ["Jwt:Algorithm"] = SecurityAlgorithms.HmacSha512,
                ["Jwt:SecondsValid"] = "100000"
            };

            var state = await ScenarioContext.Get<IWebServer>().StartAsync<Startup>(typeof(Startup).Assembly, Option.None<string>(),
                                                  configuration);

            ScenarioContext["BLOG_SERVICE_URL"] = state.Endpoint;
            ScenarioContext["WEB_CONFIGURATION"] = configuration;
        }
    }
}
