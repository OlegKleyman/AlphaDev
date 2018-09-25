using System.Linq;
using System.Text.RegularExpressions;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Web.Tests.Integration.Extensions;
using AlphaDev.Web.Tests.Integration.Fixtures;
using FluentAssertions;
using JetBrains.Annotations;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Extended;
using Markdig;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    public partial class AboutEdit_feature : WebFeatureFixture
    {
        private string _aboutValue;

        public AboutEdit_feature(ITestOutputHelper output, [NotNull] DatabaseWebServerFixture databaseWebServerFixture) :
            base(output, databaseWebServerFixture)
        {
        }

        private void When_I_click_the_edit_icon()
        {
            SiteTester.About.EditAbout();
        }

        private void Then_I_should_be_directed_to_the_edit_about_page()
        {
            SiteTester.Driver.Url.Should().BeEquivalentTo(SiteTester.About.Edit.BaseUrl.AbsoluteUri);
        }

        private void And_am_editing_the_about_info()
        {
            When_I_go_to_about_edit_page();
        }

        private void When_I_go_to_about_edit_page()
        {
            SiteTester.About.Edit.GoTo();
        }

        [UsedImplicitly]
        private CompositeStep And_I_edit_about_info()
        {
            return CompositeStep.DefineNew()
                .AddSteps(_ => And_am_editing_the_about_info(), _ => And_I_entered_markdown_content()).Build();
        }

        private void And_I_entered_markdown_content()
        {
            var about = (About)CommonSteps.Data["AddedAbout"];
            SiteTester.About.Edit.Content = _aboutValue = about.Value + "\r\n\r\n```\r\ntest\r\n```";
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

        private void And_I_empty_all_required_fields()
        {
            SiteTester.About.Edit.Content = _aboutValue = string.Empty;
        }

        private void When_I_click_save()
        {
            SiteTester.About.Edit.Submit();
        }

        private void Then_it_should_display_errors_for_the_required_fields_not_filled_in()
        {
            SiteTester.About.Edit.PageErrors.Should().BeEquivalentTo("The Value field is required.");
        }

        private void Then_I_should_be_redirected_to_the_about_page()
        {
            SiteTester.Driver.Url.Should().BeEquivalentTo(SiteTester.About.BaseUrl.AbsoluteUri.TrimEnd('/'));
        }

        private void Then_it_should_be_saved_in_the_datastore()
        {
            DatabasesFixture.InformationContextDatabaseFixture.InformationContext.Abouts.AsNoTracking().Single().Value
                .Should()
                .BeEquivalentTo(_aboutValue);
        }
    }
}