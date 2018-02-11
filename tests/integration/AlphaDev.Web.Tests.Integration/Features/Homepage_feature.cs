﻿using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
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
        [Scenario]
        public void Load_homepage()
        {
            Runner.RunScenario(
                Given_i_am_a_user,
                When_i_go_to_the_homepage,
                Then_it_should_load);
        }

        [Scenario]
        public void Display_nav()
        {
            Runner.RunScenario(
                Given_i_am_a_user,
                When_i_go_to_the_homepage,
                Then_it_should_display_navigation_links);
        }

        [Scenario]
        public void Display_latest_post()
        {
            Runner.RunScenario(
                Given_i_am_a_user,
                And_there_are_multiple_blog_posts_at_different_times,
                When_i_go_to_the_homepage,
                Then_it_should_display_the_latest_blog_post);
        }

        [Scenario]
        public void Display_dates_using_dd_for_day()
        {
            Runner.RunScenario(
                Given_i_am_a_user,
                And_there_is_a_blog_post_with_single_digit_days,
                When_i_go_to_the_homepage,
                Then_it_should_display_two_digits_for_day_for_created,
                And_it_should_display_two_digits_for_day_for_modified);
        }

        [Scenario]
        public void Display_post_with_markdown_parsed_to_html()
        {
            Runner.RunScenario(
                Given_i_am_a_user,
                And_the_latest_blog_post_contains_markdown,
                When_i_go_to_the_homepage,
                Then_it_should_display_blog_post_with_markdown_parsed_to_html);
        }

        [Scenario]
        public void Display_post_with_modification_date()
        {
            Runner.RunScenario(
                Given_i_am_a_user,
                And_the_latest_blog_post_was_modified,
                When_i_go_to_the_homepage,
                Then_it_should_display_with_modification_date);
        }

        [Scenario]
        public void Display_post_with_title()
        {
            Runner.RunScenario(
                Given_i_am_a_user,
                And_there_is_a_blog_post,
                When_i_go_to_the_homepage,
                Then_it_should_display_title);
        }

        [Scenario]
        [InlineData(true)]
        [InlineData(false)]
        public void Display_modified_date_based_on_whether_was_modified_or_not(bool modifiedState)
        {
            Runner.RunScenario(
                Given_i_am_a_user,
                And_there_are_multiple_blog_posts_at_different_times,
                () => And_the_latest_blog_post_was(modifiedState),
                When_i_go_to_the_homepage,
                () => Then_it_should_display_the_latest_blog_post_with_modification_date(modifiedState));
        }

        [Scenario]
        public void Display_error_page()
        {
            Runner.RunScenario(
                Given_i_am_a_user,
                Given_the_website_has_a_problem,
                When_i_go_to_the_homepage,
                Then_it_should_display_an_error);
        }

        [Scenario]
        public void Website_should_log_errors()
        {
            Runner.RunScenario(
                Given_i_am_a_user,
                Given_the_website_has_a_problem,
                When_i_go_to_the_homepage,
                Then_an_error_should_be_logged);
        }
    }
}