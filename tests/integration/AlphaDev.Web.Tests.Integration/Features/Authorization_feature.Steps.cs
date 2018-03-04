using System;
using AlphaDev.Web.Tests.Integration.Fixtures;
using FluentAssertions;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    public partial class Authorization_feature : WebFeatureFixture
    {
        public Authorization_feature(ITestOutputHelper output, DatabaseWebServerFixture databaseWebServerFixture) :
            base(output, databaseWebServerFixture)
        {
        }

        private void And_I_am_not_logged_in()
        {
        }

        private void Then_I_am_redirected_to_a_login_page()
        {
            SiteTester.Driver.Url.Should()
                .BeEquivalentTo(
                    new Uri(
                            new Uri(SiteTester.Login.BaseUrl.AbsoluteUri.Trim(
                                '/' /* trim last slash to connect URL get variable */)), "?ReturnUrl=%2Fadmin%2F")
                        .AbsoluteUri);
        }
    }
}