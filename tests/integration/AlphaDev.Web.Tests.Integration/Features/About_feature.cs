using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [FeatureDescription(
        @"In order to view about information
As a user
I want to have a web page where I can view it")]
    public partial class About_feature
    {
        [Scenario]
        public void About_page_should_load()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                CommonSteps.When_I_go_to_the_about_page,
                Then_it_should_load);
        }

        [Scenario]
        public void About_page_should_display_about_details()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_i_am_a_user(),
                _ => When_I_go_to_a_configured_about_page(),
                _ => Then_I_should_see_about_details());
        }

        [Scenario]
        public void
            About_page_should_display_no_details_when_there_is_no_about_information_in_the_database_and_not_logged_in()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                CommonSteps.And_there_is_no_about_information,
                CommonSteps.When_I_go_to_the_about_page,
                Then_it_should_display_no_details);
        }

        [Scenario]
        public void
            About_page_should_redirect_to_create_about_page_when_there_is_no_about_information_in_the_database_and_logged_in()
        {
            Runner.RunScenario(
                CommonSteps.Given_I_have_logged_in,
                CommonSteps.And_there_is_no_about_information,
                CommonSteps.When_I_go_to_the_about_page,
                CommonSteps.Then_I_should_be_redirected_to_the_about_create_page);
        }
    }
}