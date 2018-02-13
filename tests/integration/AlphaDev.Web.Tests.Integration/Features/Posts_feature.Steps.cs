using AlphaDev.Web.Tests.Integration.Fixtures;
using FluentAssertions;
using LightBDD.Framework.Scenarios.Basic;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    public partial class Posts_feature : WebFeatureFixture
    {
        public Posts_feature(ITestOutputHelper output, DatabaseWebServerFixture databaseWebServerFixture) : base(output, databaseWebServerFixture)
        {
        }
        
        private void When_i_go_to_the_posts_page()
        {
            SiteTester.Posts.GoTo();
        }

        private void Then_it_should_load()
        {
            SiteTester.Posts.Title.ShouldBeEquivalentTo("Posts - AlphaDev");
        }
    }
}
