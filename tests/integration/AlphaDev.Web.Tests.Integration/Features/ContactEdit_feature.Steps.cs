using System.Linq;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Web.Tests.Integration.Extensions;
using AlphaDev.Web.Tests.Integration.Fixtures;
using FluentAssertions;
using JetBrains.Annotations;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using Markdig;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    public partial class ContactEdit_feature : WebFeatureFixture
    {
        private string _value;

        public ContactEdit_feature(ITestOutputHelper output,
            [NotNull] DatabaseWebServerFixture databaseWebServerFixture) :
            base(output, databaseWebServerFixture)
        {
        }

        private void And_am_editing_the_contact_info()
        {
            When_I_go_to_contact_contact_page();
        }

        private void When_I_go_to_contact_contact_page()
        {
            SiteTester.Contact.Edit.GoTo();
        }

        [UsedImplicitly]
        private CompositeStep And_I_edit_contact_info()
        {
            return CompositeStep.DefineNew()
                .AddSteps(_ => And_am_editing_the_contact_info(), _ => And_I_entered_markdown_content()).Build();
        }

        private void And_I_entered_markdown_content()
        {
            var contact = (Contact) CommonSteps.Data["AddedContact"];
            SiteTester.Contact.Edit.Content = _value = contact.Value + "\r\n\r\n```\r\ntest\r\n```";
        }

        private void When_I_preview_the_content()
        {
            SiteTester.Contact.Edit.TogglePreview();
        }

        private void Then_it_should_be_rendered_to_html()
        {
            SiteTester.Contact.Edit.Preview.Should()
                .BeEquivalentTo(Markdown.ToHtml(_value).NormalizeToWindowsLineEndings().Trim());
        }

        private void And_I_empty_all_required_fields()
        {
            SiteTester.Contact.Edit.Content = _value = string.Empty;
        }

        private void When_I_click_save()
        {
            SiteTester.Contact.Edit.Submit();
        }

        private void Then_it_should_display_errors_for_the_required_fields_not_filled_in()
        {
            SiteTester.Contact.Edit.PageErrors.Should().BeEquivalentTo("The Value field is required.");
        }

        private void Then_I_should_be_redirected_to_the_contact_page()
        {
            SiteTester.Driver.Url.Should().BeEquivalentTo(SiteTester.Contact.BaseUrl.AbsoluteUri.TrimEnd('/'));
        }

        private void Then_it_should_be_saved_in_the_datastore()
        {
            DatabasesFixture.InformationContextDatabaseFixture.InformationContext.Contacts.AsNoTracking().Single().Value
                .Should()
                .BeEquivalentTo(_value);
        }
    }
}