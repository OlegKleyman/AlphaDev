using System.Linq;
using System.Text.RegularExpressions;
using AlphaDev.Web.Tests.Integration.Extensions;
using AlphaDev.Web.Tests.Integration.Fixtures;
using FluentAssertions;
using JetBrains.Annotations;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using Markdig;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [UsedImplicitly]
    public partial class AddBlog_feature : WebFeatureFixture
    {
        private string _addedBlogContent;
        private string _addedBlogTitle;

        public AddBlog_feature(ITestOutputHelper output, DatabaseWebServerFixture databaseWebServerFixture) : base(
            output, databaseWebServerFixture)
        {
        }

        private void When_I_fill_in_required_fields()
        {
            SiteTester.Posts.Create.BlogTitle = _addedBlogTitle = "test";
            SiteTester.Posts.Create.Content = _addedBlogContent = "testing";
        }

        [UsedImplicitly]
        private CompositeStep When_I_save_a_blog()
        {
            return CompositeStep.DefineNew()
                .AddSteps(When_I_fill_in_required_fields, When_I_click_save).Build();
        }

        private void Then_it_should_be_saved_in_the_datastore()
        {
            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs.LastOrDefault().Should()
                .BeEquivalentTo(new
                {
                    Title = _addedBlogTitle,
                    Content = _addedBlogContent
                }, options => options.ExcludingMissingMembers());
        }

        private void And_am_on_the_create_blog_page()
        {
            SiteTester.Posts.Create.GoTo();
        }

        private void And_I_entered_markdown_content()
        {
            SiteTester.Posts.Create.Content = _addedBlogContent = "```\ntest\n```";
        }

        private void When_I_preview_the_content()
        {
            SiteTester.Posts.Create.TogglePreview();
        }

        private void Then_it_should_be_rendered_to_html()
        {
            SiteTester.Posts.Create.Preview.Should()
                .BeEquivalentTo(Markdown.ToHtml(_addedBlogContent).NormalizeToWindowsLineEndings().Trim());
        }

        private void When_I_click_save()
        {
            SiteTester.Posts.Create.Submit();
        }

        private void Then_it_should_display_errors_under_the_required_fields_not_filled_in()
        {
            SiteTester.Posts.Create.PageErrors.Should()
                .BeEquivalentTo("The Title field is required.", "The Content field is required.");
        }

        private void Then_I_should_be_redirected_to_the_post()
        {
            SiteTester.Driver.Url.Should().MatchRegex(Regex.Escape(SiteTester.Posts.BaseUrl.AbsoluteUri) + @"\d+$");
        }
    }
}