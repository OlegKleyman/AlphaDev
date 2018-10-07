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
    public partial class ContactCreate_feature : WebFeatureFixture
    {
        private string _contactValue;

        public ContactCreate_feature(ITestOutputHelper output,
            [NotNull] DatabaseWebServerFixture databaseWebServerFixture) :
            base(output, databaseWebServerFixture)
        {
        }

        private void And_I_entered_markdown_content()
        {
            SiteTester.Contact.Edit.Content = _contactValue = "```\r\ntest\r\n```";
        }

        private void When_I_preview_the_content()
        {
            SiteTester.Contact.Edit.TogglePreview();
        }

        private void Then_it_should_be_rendered_to_html()
        {
            SiteTester.Contact.Edit.Preview.Should()
                .BeEquivalentTo(Markdown.ToHtml(_contactValue).NormalizeToWindowsLineEndings().Trim());
        }

        private void And_am_on_the_create_contact_page()
        {
            When_I_go_to_contact_create_page();
        }

        private void When_I_click_save()
        {
            SiteTester.Contact.Create.Submit();
        }

        private void Then_it_should_display_errors_for_the_required_fields_not_filled_in()
        {
            SiteTester.Contact.Create.PageErrors.Should().BeEquivalentTo("The Value field is required.");
        }

        private void Then_it_should_be_saved_in_the_datastore()
        {
            DatabasesFixture.InformationContextDatabaseFixture.InformationContext.Contacts.AsNoTracking().Single().Value
                .Should()
                .BeEquivalentTo(_contactValue);
        }

        private void When_I_go_to_contact_create_page()
        {
            SiteTester.Contact.Create.GoTo();
        }
    }
}