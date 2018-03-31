using System.Linq;
using System.Text.RegularExpressions;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Web.Tests.Integration.Fixtures;
using FluentAssertions;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using Markdig;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    public partial class EditBlog_feature : WebFeatureFixture
    {
        private string _blogContent;
        private string _blogTitle;

        public EditBlog_feature(ITestOutputHelper output, DatabaseWebServerFixture databaseWebServerFixture) : base(
            output, databaseWebServerFixture)
        {
        }

        private void When_I_fill_in_required_fields()
        {
            var blog = (Blog) CommonSteps.Data["AddedBlog"];

            SiteTester.Posts.Edit.BlogTitle = _blogTitle = blog.Title + "test";
            SiteTester.Posts.Edit.Content = _blogContent = blog.Content + "testing";
        }

        private CompositeStep When_I_save_a_blog()
        {
            return CompositeStep.DefineNew()
                .AddSteps(When_I_fill_in_required_fields, When_I_click_save).Build();
        }

        private void When_I_save()
        {
            SiteTester.Posts.Edit.Submit();
        }

        private void Then_it_should_be_saved_in_the_datastore()
        {
            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs.LastOrDefault().Should()
                .BeEquivalentTo(new
                {
                    Title = _blogTitle,
                    Content = _blogContent
                }, options => options.ExcludingMissingMembers());
        }

        private void And_I_entered_markdown_content()
        {
            var blog = (Blog)CommonSteps.Data["AddedBlog"];
            SiteTester.Posts.Edit.Content = _blogContent = blog.Content + "\n```\ntest\n```";
        }

        private void When_I_preview_the_content()
        {
            SiteTester.Posts.Edit.TogglePreview();
        }

        private void Then_it_should_be_rendered_to_html()
        {
            SiteTester.Posts.Edit.Preview.Should()
                .BeEquivalentTo(Markdown.ToHtml(_blogContent).Replace("\n", "\r\n").Trim());
        }

        private void When_I_click_save()
        {
            SiteTester.Posts.Edit.Submit();
        }

        private void Then_it_should_display_errors_under_the_required_fields_not_filled_in()
        {
            SiteTester.Posts.Edit.PageErrors.Should()
                .BeEquivalentTo("The Title field is required.", "The Content field is required.");
        }

        private void When_I_click_the_edit_icon()
        {
            SiteTester.HomePage.EditBlog();
        }

        private void Then_I_should_be_directed_to_the_edit_page_of_that_blog()
        {
            var blog = (Blog)CommonSteps.Data["AddedBlog"];
            SiteTester.Driver.Url.Should()
                .MatchRegex($@"{Regex.Escape(SiteTester.Posts.Edit.BaseUrl.AbsoluteUri)}/{blog.Id}");
        }

        private void And_am_editing_the_blog()
        {
            var blog = (Blog)CommonSteps.Data["AddedBlog"];
            SiteTester.Posts.Edit.GoTo(blog.Id);
        }

        private void And_I_empty_all_required_fields()
        {
            SiteTester.Posts.Edit.BlogTitle = _blogTitle = string.Empty;
            SiteTester.Posts.Edit.Content = _blogContent = string.Empty;
        }

        private void Then_I_should_be_redirected_to_the_post()
        {
            var blog = (Blog)CommonSteps.Data["AddedBlog"];
            SiteTester.Driver.Url.Should().MatchRegex($"{Regex.Escape(SiteTester.Posts.BaseUrl.AbsoluteUri)}{blog.Id}$");
        }

        private void And_I_try_to_edit_a_blog_post_that_doesnt_exist()
        {
            SiteTester.Posts.Edit.GoTo(default);
        }
    }
}