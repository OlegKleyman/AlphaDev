﻿namespace AlphaDev.Web.Tests.Integration.Features
{
    using LightBDD.Framework;
    using LightBDD.Framework.Scenarios.Extended;
    using LightBDD.XUnit2;

    [FeatureDescription(
        @"In order to see the web site
          As a user
          I want to interact with the homepage")]
    public partial class Homepage_feature
    {
        [Scenario]
        public void Load_homepage() => Runner.RunScenario(
            _ => Given_i_am_a_user(),
            _ => When_i_go_to_the_homepage(),
            _ => Then_it_should_load());

        [Scenario]
        public void Display_nav() => Runner.RunScenario(
            _ => Given_i_am_a_user(),
            _ => When_i_go_to_the_homepage(),
            _ => Then_it_should_display_navigation_links());

        [Scenario]
        public void Display_latest_post() => Runner.RunScenario(
            _ => Given_i_am_a_user(),
            _ => When_i_go_to_the_homepage(),
            _ => Then_it_should_display_the_latest_blog_post());
    }
}