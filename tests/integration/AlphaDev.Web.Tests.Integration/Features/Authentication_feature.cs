using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [FeatureDescription(
        @"In order to have authorization to protected parts of the web site
As a user
I want to authenticate myself")]
    public partial class Authentication_feature
    {
        [Scenario]
        public void Login_page_should_load()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                When_I_go_to_the_login_page,
                Then_it_should_load);
        }


        [Scenario]
        public void Login_page_should_redirect_back_to_protected_page_after_successful_login()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                And_visited_a_protected_area,
                When_I_successfully_login,
                Then_I_should_be_redirected_back_to_the_admin_area);
        }


        [Scenario]
        public void Login_page_should_show_validation_errors_when_required_fields_are_not_filled()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                When_I_go_to_the_login_page,
                And_submit_the_login_form,
                Then_it_should_display_errors_for_the_required_field);
        }
    }
}