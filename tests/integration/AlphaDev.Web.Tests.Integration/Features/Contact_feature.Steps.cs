using System.Linq;
using AlphaDev.Web.Tests.Integration.Extensions;
using AlphaDev.Web.Tests.Integration.Fixtures;
using FluentAssertions;
using JetBrains.Annotations;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using Markdig;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [UsedImplicitly]
    public partial class Contact_feature : WebFeatureFixture
    {
        public Contact_feature(ITestOutputHelper output, [NotNull] DatabaseWebServerFixture databaseWebServerFixture) :
            base(output, databaseWebServerFixture)
        {
        }

        private void Then_it_should_load()
        {
            SiteTester.About.Title.Should().BeEquivalentTo("Contact - AlphaDev");
        }

        [UsedImplicitly]
        private CompositeStep When_I_go_to_a_configured_contact_page()
        {
            return CompositeStep.DefineNew()
                                .AddSteps(_ => CommonSteps.And_there_is_contact_information(),
                                    _ => CommonSteps.When_I_go_to_the_contact_page())
                                .Build();
        }

        private void Then_i_should_see_contact_details()
        {
            SiteTester.Contact.Details.Should()
                      .BeEquivalentTo(Markdown.ToHtml(DatabasesFixture
                                                      .InformationContextDatabaseFixture
                                                      .InformationContext.Contacts.Single()
                                                      .Value)
                                              .NormalizeToWindowsLineEndings()
                                              .Trim());
        }

        private void Then_it_should_display_no_details()
        {
            SiteTester.Contact.Details.Should()
                      .BeEquivalentTo("No details".ToHtmlFromMarkdown().NormalizeToWindowsLineEndings().Trim());
        }

        private void When_I_click_the_edit_icon()
        {
            SiteTester.Contact.EditValue();
        }

        private void Then_I_should_be_directed_to_the_edit_contact_page()
        {
            SiteTester.Driver.Url.Should().BeEquivalentTo(SiteTester.Contact.Edit.BaseUrl.AbsoluteUri);
        }
    }
}