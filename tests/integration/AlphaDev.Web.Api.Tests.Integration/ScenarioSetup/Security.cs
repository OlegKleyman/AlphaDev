using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Web.Api.Tests.Integration.Support;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechTalk.SpecFlow;

namespace AlphaDev.Web.Api.Tests.Integration.ScenarioSetup
{
    [Binding]
    public class Security : Steps
    {
        public Security([NotNull] ScenarioContext scenarioContext, IdentityDbContext<User> context) => scenarioContext.Set(context);

        [BeforeScenario("security", Order = (int) ScenarioOrder.Security)]
        public void InitializeDatabase()
        {
            ScenarioContext.Get<IdentityDbContext<User>>().Database.Migrate();
        }
    }
}
