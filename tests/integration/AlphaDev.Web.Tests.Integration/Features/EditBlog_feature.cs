using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [FeatureDescription(
        @"In order to edit a blog
As an administrator
I want to have a an edit icon on every blog post that leads me to edit a blog")]
    public partial class EditBlog_feature
    {
        [Scenario]
        public void Edit_icon_should_link_to_edit_post_page()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_I_have_logged_in(),
                _ => CommonSteps.And_there_is_a_blog_post(),
                _ => CommonSteps.And_I_am_on_the_homepage(),
                _ => When_I_click_the_edit_icon(),
                _ => Then_I_should_be_directed_to_the_edit_page_of_that_blog());
        }

        [Scenario]
        public void Preview_blog_post()
        {
            Runner.RunScenario(
                CommonSteps.Given_I_have_logged_in,
                CommonSteps.And_there_is_a_blog_post,
                And_am_editing_the_blog,
                And_I_entered_markdown_content,
                When_I_preview_the_content,
                Then_it_should_be_rendered_to_html);
        }

        [Scenario]
        public void Show_errors_when_required_fields_are_not_filled()
        {
            Runner.RunScenario(
                CommonSteps.Given_I_have_logged_in,
                CommonSteps.And_there_is_a_blog_post,
                And_am_editing_the_blog,
                And_I_empty_all_required_fields,
                When_I_click_save,
                Then_it_should_display_errors_under_the_required_fields_not_filled_in);
        }

        [Scenario]
        public void After_editing_a_post_I_should_be_redirected_to_it()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_I_have_logged_in(),
                _ => CommonSteps.And_there_is_a_blog_post(),
                _ => And_am_editing_the_blog(),
                _ => When_I_save_a_blog(),
                _ => Then_I_should_be_redirected_to_the_post());
        }

        [Scenario]
        public void When_I_try_to_edit_a_post_that_doesnt_exist_I_should_get_a_404()
        {
            Runner.RunScenario(
                _ => CommonSteps.Given_I_have_logged_in(),
                _ => And_I_try_to_edit_a_blog_post_that_doesnt_exist(),
                _ => CommonSteps.Then_it_should_display_the_error_page_with_a_404());
        }
    }
}