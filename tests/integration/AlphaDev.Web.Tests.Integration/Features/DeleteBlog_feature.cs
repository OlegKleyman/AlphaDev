using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [FeatureDescription(
        @"In order to delete blogs
As an admin
I want to use a link on a blog to do it")]
    public partial class DeleteBlog_feature
    {
        [Scenario]
        public void Delete_blog_on_homepage()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_there_is_a_blog_post(),
                _ => CommonSteps.And_I_am_on_the_homepage(),
                _ => When_I_click_the_delete_symbol(),
                _ => Then_it_should_be_deleted());
        }

        [Scenario]
        public void Delete_blog_on_post()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_there_is_a_blog_post(),
                _ => CommonSteps.And_I_am_on_the_blog_post_page(),
                _ => When_I_click_the_delete_symbol(),
                _ => Then_it_should_be_deleted());
        }

        [Scenario]
        public void Delete_blog_on_posts_preview()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_there_is_a_blog_post(),
                _ => CommonSteps.And_I_am_on_the_blog_posts_page(),
                _ => When_I_click_the_delete_symbol(),
                _ => Then_it_should_be_deleted());
        }

        [Scenario]
        public void Deleting_a_blog_should_direct_to_posts_page()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_there_is_a_blog_post(),
                _ => CommonSteps.And_I_am_on_the_blog_posts_page(),
                _ => When_I_click_the_delete_symbol(),
                _ => Then_should_be_directed_to_the_posts_page());
        }
    }
}