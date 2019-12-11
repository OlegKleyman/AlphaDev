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
                                .AddSteps(_ => CommonSteps.And_there_is_about_information(),
                                    _ => CommonSteps.When_I_go_to_the_about_page())
                                .Build();
        }

        private void Then_it_should_load()
        {
            SiteTester.About.Title.Should().BeEquivalentTo("About - AlphaDev");
        }

        private void Then_I_should_see_about_details()
        {
            SiteTester.About.Details.Should()
                      .BeEquivalentTo(Markdown.ToHtml(DatabasesFixture
                                                      .InformationContextDatabaseFixture
                                                      .InformationContext.Abouts.Single()
                                                      .Value)
                                              .NormalizeToWindowsLineEndings()
                                              .Trim());
        }

        private void Then_it_should_display_no_details()
        {
            SiteTester.About.Details.Should()
                      .BeEquivalentTo("No details".ToHtmlFromMarkdown().NormalizeToWindowsLineEndings().Trim());
        }
    }
}