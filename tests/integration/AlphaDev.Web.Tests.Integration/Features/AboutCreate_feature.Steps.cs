using System.Linq;
using AlphaDev.Web.Tests.Integration.Extensions;
using AlphaDev.Web.Tests.Integration.Fixtures;
using FluentAssertions;
using JetBrains.Annotations;
using Markdig;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [UsedImplicitly]
    public partial class AboutCreate_feature : WebFeatureFixture
    {
        private string _aboutValue;

        public AboutCreate_feature(ITestOutputHelper output,
            [NotNull] DatabaseWebServerFixture databaseWebServerFixture) :
            base(output, databaseWebServerFixture)
        {
        }

        private void And_I_entered_markdown_content()
        {
            SiteTester.About.Edit.Content = _aboutValue = "```\r\ntest\r\n```";
        }

        private void When_I_preview_the_content()
        {
            SiteTester.About.Edit.TogglePreview();
        }

        private void Then_it_should_be_rendered_to_html()
        {
            SiteTester.About.Edit.Preview.Should()
                .BeEquivalentTo(Markdown.ToHtml(_aboutValue).NormalizeToWindowsLineEndings().Trim());
        }

        private void And_am_on_the_create_about_page()
        {
            When_I_go_to_about_create_page();
        }

        private void When_I_click_save()
        {
            SiteTester.About.Create.Submit();
        }

        private void Then_it_should_display_errors_for_the_required_fields_not_filled_in()
        {
            SiteTester.About.Create.PageErrors.Should().BeEquivalentTo("The Value field is required.");
        }

        private void Then_it_should_be_saved_in_the_datastore()
        {
            DatabasesFixture.InformationContextDatabaseFixture.InformationContext.Abouts.AsNoTracking().Single().Value
                .Should()
                .BeEquivalentTo(_aboutValue);
        }

        private void When_I_go_to_about_create_page()
        {
            SiteTester.About.Create.GoTo();
        }
    }
}