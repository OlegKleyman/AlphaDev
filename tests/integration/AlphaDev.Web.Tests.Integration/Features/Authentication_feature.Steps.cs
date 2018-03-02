using AlphaDev.Web.Tests.Integration.Fixtures;
using FluentAssertions;
using LightBDD.Framework;
using LightBDD.XUnit2;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
	public partial class Authentication_feature: WebFeatureFixture
	{
	    public Authentication_feature(ITestOutputHelper output, DatabaseWebServerFixture databaseWebServerFixture) : base(output, databaseWebServerFixture)
	    {
	    }

	    private void When_I_go_to_the_login_page()
	    {
	        SiteTester.Login.GoTo();
	    }

	    private void Then_it_should_load()
	    {
	        SiteTester.Login.Title.Should().BeEquivalentTo("Login - AlphaDev");
	    }
	}
}