using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;

namespace AlphaDev.Web.Tests.Integration.Features
{
	[FeatureDescription(
@"In order to know what a particular post's content is
As a user
I want to view the post")]
	public partial class Post_feature
	{
	    [Scenario]
	    public void Posts_page_should_display_single_post_when_viewing_by_id()
	    {
	        Runner.RunScenario(
	            _ => CommonSteps.Given_i_am_a_user(),
                _ => When_I_view_a_post_by_id(),
	            _ => Then_it_should_load_the_post());
	    }
    }
}