using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;
using Xunit;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [FeatureDescription(
        @"In order to see the web site
          As a user
          I want to interact with the homepage")]
    public partial class Homepage_feature
    {
        [Scenario]
        public void Load_homepage()
        {
            Runner.RunScenario(
                Given_i_am_a_user,
                When_i_go_to_the_homepage,
                Then_it_should_load);
        }

        [Scenario]
        public void Display_nav()
        {
            Runner.RunScenario(
                Given_i_am_a_user,
                When_i_go_to_the_homepage,
                Then_it_should_display_navigation_links);
        }

        [Scenario]
        public void Display_latest_post()
        {
            Runner.RunScenario(
                Given_i_am_a_user,
                When_i_go_to_the_homepage,
                Then_it_should_display_the_latest_blog_post);
        }

        [Scenario]
        [InlineData(true)]
        [InlineData(false)]
        public void Display_modified_date_based_on_whether_was_modified_or_not(bool modifiedState)
        {
            Runner.RunScenario(
                Given_i_am_a_user,
                () => And_the_latest_blog_post_was(modifiedState),
                When_i_go_to_the_homepage,
                () => Then_it_should_display_the_latest_blog_post_with_modification_date(modifiedState));
        }

        [Scenario]
        public void Display_error_page()
        {
            Runner.RunScenario(
                Given_i_am_a_user,
                Given_the_website_has_a_problem,
                When_i_go_to_the_homepage,
                Then_it_should_display_an_error);
        }

        [Scenario]
        public void Website_should_log_errors()
        {
            Runner.RunScenario(
                Given_i_am_a_user,
                Given_the_website_has_a_problem,
                When_i_go_to_the_homepage,
                Then_an_error_should_be_logged);
        }
    }
}