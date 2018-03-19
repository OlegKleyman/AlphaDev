using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;

namespace AlphaDev.Web.Tests.Integration.Features
{
	[FeatureDescription(
@"In order to add a blog
As an administrator
I want to have a web page where I can create a blog post")]
	public partial class AddBlog_feature
	{
		[Scenario]
		public void Create_a_blog_post()
		{
			Runner.RunScenario(
				CommonSteps.Given_I_have_logged_in,
                And_am_on_the_create_blog_page,
				When_I_submit_a_blog,
			    Then_it_should_be_saved_in_the_datastore);
		}
	}
}