using System;
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

	    private void And_visited_a_protected_area()
	    {
	        CommonSteps.When_I_visit_the_admin_area();
	    }

	    private void When_I_successfully_login()
	    {
	        SiteTester.Login.Username = "something@something.com";
	        SiteTester.Login.Password = "H3ll04321!";

	        SiteTester.Login.Submit();
	    }

	    private void Then_I_should_be_redirected_back_to_the_admin_area()
	    {
	        SiteTester.Driver.Url.Should()
	            .BeEquivalentTo(
	                SiteTester.Admin.BaseUrl
                        .AbsoluteUri);
        }

	    private void And_submit_the_login_form()
	    {
	        SiteTester.Login.Submit();
	    }

	    private void Then_it_should_display_errors_for_the_required_field()
	    {
	        SiteTester.Login.ValidationSummary.Should()
	            .BeEquivalentTo("The Username field is required.", "The Password field is required.");
	    }
	}
}