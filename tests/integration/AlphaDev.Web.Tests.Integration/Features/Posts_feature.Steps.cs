using AlphaDev.Web.Tests.Integration.Fixtures;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    public partial class Posts_feature : WebFeatureFixture
    {
        public Posts_feature(ITestOutputHelper output, DatabaseWebServerFixture databaseWebServerFixture) : base(output, databaseWebServerFixture)
        {
        }
    }
}
