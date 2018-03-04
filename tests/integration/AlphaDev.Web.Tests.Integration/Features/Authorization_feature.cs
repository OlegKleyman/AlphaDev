using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [FeatureDescription(
        @"In order to not allow everyone to access the admin area
As a user
I want to only be allowed to access the admin area if I have the authority")]
    public partial class Authorization_feature
    {
        [Scenario]
        public void Redirect_unauthenticated_users_to_login_page()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_i_am_a_user(),
                _ => And_I_am_not_logged_in(),
                _ => CommonSteps.When_I_visit_the_admin_area(),
                _ => Then_I_am_redirected_to_a_login_page());
        }
    }
}