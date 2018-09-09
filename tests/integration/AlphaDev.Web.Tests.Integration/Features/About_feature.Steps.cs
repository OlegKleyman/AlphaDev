using System.Linq;
using AlphaDev.Web.Tests.Integration.Fixtures;
using FluentAssertions;
using JetBrains.Annotations;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Extended;
using Markdig;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    public partial class About_feature : WebFeatureFixture
    {
        public About_feature(ITestOutputHelper output, [NotNull] DatabaseWebServerFixture databaseWebServerFixture) :
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

        private void Then_it_should_load()
        {
            SiteTester.About.Title.Should().BeEquivalentTo("About - AlphaDev");
        }

        private void Then_I_should_see_about_details()
        {
            SiteTester.About.Details.Should().BeEquivalentTo(Markdown.ToHtml(DatabasesFixture
                .InformationContextDatabaseFixture
                .InformationContext.Abouts.Single().Value).Trim());
        }
    }
}