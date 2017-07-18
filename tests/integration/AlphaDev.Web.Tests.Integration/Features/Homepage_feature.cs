using Xunit;

namespace AlphaDev.Web.Tests.Integration.Features
{
    using LightBDD.Framework;
    using LightBDD.Framework.Scenarios.Basic;
    using LightBDD.Framework.Scenarios.Extended;
    using LightBDD.XUnit2;

    [FeatureDescription(
        @"In order to see the web site
          As a user
          I want to interact with the homepage")]
    public partial class Homepage_feature
    {
        [Scenario]
        public void Load_homepage() => Runner.RunScenario(
            Given_i_am_a_user,
            When_i_go_to_the_homepage,
            Then_it_should_load);

        [Scenario]
        public void Display_nav() => Runner.RunScenario(
            Given_i_am_a_user,
            When_i_go_to_the_homepage,
            Then_it_should_display_navigation_links);

        [Scenario]
        public void Display_latest_post() => Runner.RunScenario(
            Given_i_am_a_user,
            When_i_go_to_the_homepage,
            Then_it_should_display_the_latest_blog_post);

        [Scenario]
        [InlineData(true)]
        [InlineData(false)]
        public void Display_modified_date_based_on_whether_was_modified_or_not(bool modifiedState) => Runner.RunScenario(
            Given_i_am_a_user,
            () => And_the_latest_blog_post_was(modifiedState),
            When_i_go_to_the_homepage,
            () => Then_it_should_display_the_latest_blog_post_with_modification_date(modifiedState));
    }
}