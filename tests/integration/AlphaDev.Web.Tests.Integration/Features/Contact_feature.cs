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
    }
}