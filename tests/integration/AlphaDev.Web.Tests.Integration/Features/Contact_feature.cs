using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [FeatureDescription(
        @"In order to view contact information
As a user
I want to have a web page where I can view it")]
    public partial class Contact_feature
    {
        [Scenario]
        public void Contact_page_should_load()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                CommonSteps.When_I_go_to_the_contact_page,
                Then_it_should_load);
        }

        [Scenario]
        public void About_page_should_display_about_details()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_i_am_a_user(),
                _ => When_I_go_to_a_configured_contact_page(),
                _ => Then_i_should_see_contact_details());
        }

        [Scenario]
        public void
            Contact_page_should_display_no_details_when_there_is_no_contact_information_in_the_database_and_not_logged_in()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                CommonSteps.And_there_is_no_contact_information,
                CommonSteps.When_I_go_to_the_contact_page,
                Then_it_should_display_no_details);
        }

        [Scenario]
        public void
            Contact_page_should_redirect_to_create_about_page_when_there_is_no_contact_information_in_the_database_and_logged_in()
        {
            Runner.RunScenario(
                CommonSteps.Given_I_have_logged_in,
                CommonSteps.And_there_is_no_contact_information,
                CommonSteps.When_I_go_to_the_contact_page,
                CommonSteps.Then_I_should_be_redirected_to_the_contact_create_page);
        }

        [Scenario]
        public void Edit_icon_should_link_to_edit_about_page()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_I_have_logged_in(),
                _ => CommonSteps.And_there_is_contact_information(),
                _ => CommonSteps.And_I_am_on_the_contact_page(),
                _ => When_I_click_the_edit_icon(),
                _ => Then_I_should_be_directed_to_the_edit_contact_page());
        }
    }
}