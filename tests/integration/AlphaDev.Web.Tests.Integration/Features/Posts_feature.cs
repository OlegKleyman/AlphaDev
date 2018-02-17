using AlphaDev.Web.Tests.Integration.Support;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;
using LightBDD.Framework.Scenarios.Extended;
using Xunit;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [FeatureDescription(
        @"In order to see blog posts
          As a user
          I want to interact with the posts page")]
    public partial class Posts_feature
    {
        [Scenario]
        public void Posts_page_should_load()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                When_i_go_to_the_posts_page,
                Then_it_should_load);
        }

        [Scenario]
        public void Posts_page_should_load_all_posts()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                And_there_are_multiple_posts,
                When_i_go_to_the_posts_page,
                Then_it_should_display_all_posts);
        }

        [Scenario]
        public void Posts_page_should_load_all_posts_ordered_by_creation_date_descending()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                And_there_are_multiple_posts,
                When_i_go_to_the_posts_page,
                Then_it_should_display_all_posts_ordered_by_creation_date_descending);
        }

        [Scenario]
        public void Posts_page_should_load_all_posts_with_markdown_parsed_to_html()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                And_there_are_multiple_posts_with_markdown,
                When_i_go_to_the_posts_page,
                Then_it_should_display_all_posts_with_markdown_parsed_to_html);
        }

        [Scenario]
        public void Posts_page_should_load_all_posts_with_a_title()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                And_there_are_multiple_posts,
                When_i_go_to_the_posts_page,
                Then_it_should_display_all_posts_with_a_title);
        }

        [Scenario]
        [InlineData(ModifiedState.Modified)]
        [InlineData(ModifiedState.NotModified)]
        public void Display_modified_date_for_all_posts_based_on_whether_was(ModifiedState modifiedState)
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_i_am_a_user(),
                _ => And_all_posts_were(modifiedState),
                _ => When_i_go_to_the_posts_page(),
                _ => Then_it_should_display_all_posts_with_modification_date_if_it_exists());
        }

        [Scenario]
        public void Posts_page_should_load_all_posts_with_a_navigation_link_to_the_entire_post()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                And_there_are_multiple_posts,
                When_i_go_to_the_posts_page,
                Then_it_should_display_all_posts_with_a_navigation_link_to_the_entire_post);
        }
    }
}
