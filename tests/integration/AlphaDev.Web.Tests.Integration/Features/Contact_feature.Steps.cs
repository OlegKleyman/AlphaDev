using System.Linq;
using AlphaDev.Web.Tests.Integration.Extensions;
using AlphaDev.Web.Tests.Integration.Fixtures;
using FluentAssertions;
using JetBrains.Annotations;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Extended;
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
                    _ => CommonSteps.When_I_go_to_the_contact_page()).Build();
        }

        private void Then_i_should_see_contact_details()
        {
            SiteTester.Contact.Details.Should().BeEquivalentTo(Markdown.ToHtml(DatabasesFixture
                .InformationContextDatabaseFixture
                .InformationContext.Contacts.Single().Value).NormalizeToWindowsLineEndings().Trim());
        }
    }
}