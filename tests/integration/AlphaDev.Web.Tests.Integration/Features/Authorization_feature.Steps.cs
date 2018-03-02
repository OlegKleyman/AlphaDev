using System;
using AlphaDev.Web.Tests.Integration.Fixtures;
using FluentAssertions;
using LightBDD.Framework;
using LightBDD.XUnit2;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
	public partial class Authorization_feature: WebFeatureFixture
    {
        public Authorization_feature(ITestOutputHelper output, DatabaseWebServerFixture databaseWebServerFixture) : base(output, databaseWebServerFixture)
        {
        }

        private void And_I_am_not_logged_in()
        {
            
        }

        private void When_I_visit_the_admin_area()
        {
            SiteTester.Admin.GoTo();
        }

        private void Then_I_am_redirected_to_a_login_page()
        {
            SiteTester.Driver.Url.Should().BeEquivalentTo(SiteTester.Login.BaseUrl.AbsoluteUri);
        }
    }
}