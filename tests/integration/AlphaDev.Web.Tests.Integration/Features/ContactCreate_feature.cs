using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [FeatureDescription(
        @"In order to create contact information
As an administrator
I want to have a web page where I can create it")]
    public partial class ContactCreate_feature
    {
        [Scenario]
        public void Preview_contact()
        {
            Runner.RunScenario(
                CommonSteps.Given_I_have_logged_in,
                CommonSteps.And_there_is_no_contact_information,
                And_am_on_the_create_contact_page,
                And_I_entered_markdown_content,
                When_I_preview_the_content,
                Then_it_should_be_rendered_to_html);
        }

        [Scenario]
        public void Show_errors_when_required_fields_are_not_filled()
        {
            Runner.RunScenario(
                CommonSteps.Given_I_have_logged_in,
                CommonSteps.And_there_is_no_contact_information,
                And_am_on_the_create_contact_page,
                When_I_click_save,
                Then_it_should_display_errors_for_the_required_fields_not_filled_in);
        }

        [Scenario]
        public void Create_contact()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_I_have_logged_in(),
                _ => CommonSteps.And_there_is_no_contact_information(),
                _ => And_am_on_the_create_contact_page(),
                _ => And_I_entered_markdown_content(),
                _ => When_I_click_save(),
                _ => Then_it_should_be_saved_in_the_datastore());
        }

        [Scenario]
        public void Create_page_should_redirect_to_edit_contact_page_when_there_is_contact_information()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_I_have_logged_in(),
                _ => CommonSteps.And_there_is_contact_information(),
                _ => When_I_go_to_contact_create_page(),
                _ => CommonSteps.Then_I_should_be_redirected_to_the_contact_edit_page());
        }
    }
}