using AlphaDev.Web.Tests.Integration.Support;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;
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
                CommonSteps.And_there_is_a_blog_post,
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
        public void Display_modified_date_for_posts_based_on_whether_was(ModifiedState modifiedState)
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

        [Scenario]
        public void Posts_page_should_load_pages_without_ellipses_when_there_are_not_too_many_pages()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_i_am_a_user(),
                _ => And_there_are_PAGES_pages_of_posts(5),
                _ => When_i_go_to_the_posts_page(),
                _ => Then_it_should_display_all_the_pages());
        }

        [Scenario]
        public void Posts_page_should_have_ellipses_when_there_are_too_many_pages()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_i_am_a_user(),
                _ => And_there_are_PAGES_pages_of_posts(11),
                _ => When_i_go_to_the_posts_page(),
                _ => Then_it_should_display_ellipses_after_the_last_max_page());
        }

        [Scenario]
        public void Posts_page_should_display_the_current_page_grayed_out()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                CommonSteps.And_there_is_a_blog_post,
                When_i_go_to_the_posts_page,
                Then_it_should_display_the_current_page_as_grayed_out);
        }

        [Scenario]
        public void Posts_page_should_have_page_link_navigate_to_the_right_page()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_i_am_a_user(),
                _ => And_there_are_multiple_pages_of_posts(),
                _ => When_i_go_to_the_posts_page(),
                _ => When_i_go_to_the_PAGE_page(2),
                _ => Then_the_current_page_should_be_the_navigated_to_page());
        }

        [Scenario]
        public void Posts_page_should_load_previous_pages_before_the_current_page()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_i_am_a_user(),
                _ => And_there_are_PAGES_pages_of_posts(20),
                _ => When_i_go_to_the_posts_page(),
                _ => When_i_go_to_the_PAGE_page(14),
                _ => Then_it_should_display_pages_before_the_current_page());
        }
    }
}