using AlphaDev.Web.Tests.Integration.Support;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;
using Xunit;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [FeatureDescription(
        @"In order to know what a particular post's content is
As a user
I want to view the post")]
    public partial class Post_feature
    {
        [Scenario]
        public void Posts_page_should_load_single_post_when_viewing_by_id()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_i_am_a_user(),
                _ => When_I_view_a_post_by_id(),
                _ => Then_it_should_load_the_page());
        }

        [Scenario]
        public void Posts_page_should_display_title()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_i_am_a_user(),
                _ => When_I_view_a_post_by_id(),
                _ => Then_it_should_display_the_title());
        }

        [Scenario]
        public void Display_post_with_markdown_parsed_to_html()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_i_am_a_user(),
                _ => When_I_view_a_post_by_id(),
                _ => Then_it_should_display_blog_post_with_markdown_parsed_to_html());
        }

        [Scenario]
        [InlineData(ModifiedState.Modified)]
        [InlineData(ModifiedState.NotModified)]
        public void Display_modified_date_based_on_whether_was(ModifiedState modifiedState)
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_i_am_a_user(),
                _ => Given_there_is_a_blog_post("testing"),
                _ => And_the_blog_post_was(modifiedState),
                _ => When_I_view_it_by_id(),
                _ => Then_it_should_display_the_blog_with_a_modification_date_if_it_exists());
        }

        [Scenario]
        public void Posts_page_should_display_created_date()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_i_am_a_user(),
                _ => When_I_view_a_post_by_id(),
                _ => Then_it_should_display_the_created_date());
        }

        [Scenario]
        public void Posts_page_should_display_404_status_when_blog_is_not_found()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_i_am_a_user(),
                _ => When_I_try_to_view_a_post_that_doesnt_exist(),
                _ => Then_it_should_display_the_error_page_with_a_404_status());
        }

        [Scenario]
        public void Post_page_should_have_all_posts_menu_link_lead_to_all_posts_page()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_i_am_a_user(),
                _ => When_I_view_a_post_by_id(),
                _ => Then_it_should_display_the_posts_menu_link_to_lead_to_all_posts());
        }
    }
}