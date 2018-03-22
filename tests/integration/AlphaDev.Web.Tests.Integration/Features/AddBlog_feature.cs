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
        public void Show_errors_when_required_fields_are_not_filled()
        {
            Runner.RunScenario(
                CommonSteps.Given_I_have_logged_in,
                And_am_on_the_create_blog_page,
                When_I_click_save,
                Then_it_should_display_errors_under_the_required_fields_not_filled_in);
        }
    }
}