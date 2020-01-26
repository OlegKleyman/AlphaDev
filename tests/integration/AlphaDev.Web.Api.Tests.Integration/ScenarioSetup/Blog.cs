using AlphaDev.Core.Data.Contexts;
using AlphaDev.Web.Api.Tests.Integration.Support;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using TechTalk.SpecFlow;

namespace AlphaDev.Web.Api.Tests.Integration.ScenarioSetup
{
    [Binding]
    public class Blog : Steps
    {
        public Blog([NotNull] ScenarioContext scenarioContext, BlogContext blogContext) => scenarioContext.Set(blogContext);

        [BeforeScenario("blog", Order = (int) ScenarioOrder.Blog)]
        public void InitializeDatabase()
        {
            ScenarioContext.Get<BlogContext>().Database.Migrate();
        }
    }
}
