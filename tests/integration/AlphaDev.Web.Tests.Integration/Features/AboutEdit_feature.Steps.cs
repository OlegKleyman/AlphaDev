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

        [UsedImplicitly]
        private CompositeStep When_I_go_to_a_configured_about_page()
        {
            return CompositeStep.DefineNew()
                .AddSteps(_ => There_is_about_information(), _ => When_I_go_to_the_about_page()).Build();
        }

        private void There_is_about_information()
        {
            DatabasesFixture.InformationContextDatabaseFixture.InformationContext.Abouts.Add(DatabasesFixture
                .InformationContextDatabaseFixture.DefaultAbout);
            DatabasesFixture.InformationContextDatabaseFixture.InformationContext.SaveChanges();
        }

        private void When_I_go_to_the_about_page()
        {
            SiteTester.About.GoTo();
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
            SiteTester.About.Edit.GoTo();
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

        private void Then_it_should_display_errors_under_the_required_fields_not_filled_in()
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