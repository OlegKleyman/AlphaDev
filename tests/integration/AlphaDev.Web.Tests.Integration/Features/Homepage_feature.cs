using AlphaDev.Web.Tests.Integration.Support;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;
using Xunit;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [FeatureDescription(
        @"In order to see the web site
          As a user
          I want to interact with the homepage")]
    public partial class Homepage_feature
    {
        [Scenario][IgnoreScenario("dfsdgdfs")]
        public void Load_homepage()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                When_i_go_to_the_homepage,
                Then_it_should_load);
        }

        [Scenario][IgnoreScenario("dfsdgdfs")]
        public void Display_nav()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                When_i_go_to_the_homepage,
                Then_it_should_display_navigation_links);
        }

        [Scenario][IgnoreScenario("dfsdgdfs")]
        public void Display_latest_post()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                And_there_are_multiple_blog_posts_at_different_times,
                When_i_go_to_the_homepage,
                Then_it_should_display_the_latest_blog_post);
        }

        [Scenario][IgnoreScenario("dfsdgdfs")]
        public void Display_dates_using_dd_for_day()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                And_there_is_a_blog_post_with_single_digit_days,
                When_i_go_to_the_homepage,
                Then_it_should_display_two_digits_for_day_for_created,
                And_it_should_display_two_digits_for_day_for_modified);
        }

        [Scenario][IgnoreScenario("dfsdgdfs")]
        public void Display_post_with_markdown_parsed_to_html()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                And_the_latest_blog_post_contains_markdown,
                When_i_go_to_the_homepage,
                Then_it_should_display_blog_post_with_markdown_parsed_to_html);
        }

        [Scenario][IgnoreScenario("dfsdgdfs")]
        public void Display_post_with_title()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                And_there_is_a_blog_post,
                When_i_go_to_the_homepage,
                Then_it_should_display_title);
        }

        [Scenario][IgnoreScenario("dfsdgdfs")]
        [InlineData(ModifiedState.Modified)]
        [InlineData(ModifiedState.NotModified)]
        public void Display_modified_date_based_on_whether_was(ModifiedState modifiedState)
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_i_am_a_user(),
                _ => And_there_are_multiple_blog_posts_at_different_times(),
                _ => And_the_latest_blog_post_was(modifiedState),
                _ => When_i_go_to_the_homepage(),
                _ => Then_it_should_display_the_blog_with_a_modification_date_if_it_exists());
        }

        [Scenario][IgnoreScenario("dfsdgdfs")]
        public void Display_error_page()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                Given_the_website_has_a_problem,
                When_i_go_to_the_homepage,
                Then_it_should_display_an_error);
        }

        [Scenario][IgnoreScenario("dfsdgdfs")]
        public void Website_should_log_errors()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                Given_the_website_has_a_problem,
                When_i_go_to_the_homepage,
                Then_an_error_should_be_logged);
        }

        [Scenario][IgnoreScenario("dfsdgdfs")]
        public void Display_homepage_with_welcome_post_if_no_posts_exist()
        {
            Runner.RunScenario(
                CommonSteps.Given_i_am_a_user,
                And_there_are_no_blog_posts,
                When_i_go_to_the_homepage,
                Then_it_should_display_welcome_post);
        }
    }
}