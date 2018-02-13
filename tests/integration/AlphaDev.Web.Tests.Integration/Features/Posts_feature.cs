using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [FeatureDescription(
        @"In order to see blog posts
          As a user
          I want to interact with the posts page")]
    public partial class Posts_feature
    {
        [Scenario]
        public void Posts_page_should_load()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                When_i_go_to_the_posts_page,
                Then_it_should_load);
        }
    }
}
