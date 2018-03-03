using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.Framework.Scenarios.Extended;
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
	}
}