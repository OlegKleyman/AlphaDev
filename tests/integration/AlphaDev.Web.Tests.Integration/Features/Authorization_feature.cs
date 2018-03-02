using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;

namespace AlphaDev.Web.Tests.Integration.Features
{
	[Label("FEAT-1")]
	[FeatureDescription(
@"In order to not allow everyone to access the admin area
As a user
I want to only be allowed to access the admin area if I have the authority")]
	public partial class Authorization_feature
	{
		[Scenario]
		public void Template_basic_scenario()
		{
			Runner.RunScenario(
				_ => CommonSteps.Given_i_am_a_user(),
				_ => And_I_am_not_logged_in(),
				_=> When_I_visit_the_admin_area(),
			    _=> Then_I_am_redirected_to_a_login_page());
		}
	}
}