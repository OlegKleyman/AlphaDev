using System.Linq;
using AlphaDev.Web.Tests.Integration.Fixtures;
using FluentAssertions;
using LightBDD.Framework;
using LightBDD.XUnit2;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
	public partial class AddBlog_feature: WebFeatureFixture
	{
	    private string _addedBlogTitle;
	    private string _addedBlogContent;

	    public AddBlog_feature(ITestOutputHelper output, DatabaseWebServerFixture databaseWebServerFixture) : base(output, databaseWebServerFixture)
	    {
	    }

	    private void When_I_submit_a_blog()
	    {
	        SiteTester.Posts.Create.BlogTitle = _addedBlogTitle = "test";
	        SiteTester.Posts.Create.Content = _addedBlogContent = "testing";
	        SiteTester.Posts.Create.Submit();
	    }

	    private void Then_it_should_be_saved_in_the_datastore()
	    {
	        DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs.LastOrDefault().Should()
	            .BeEquivalentTo(new
	            {
                    Title = _addedBlogTitle,
                    Content = _addedBlogContent
	            }, options => options.ExcludingMissingMembers());
	    }

	    private void And_am_on_the_create_blog_page()
	    {
	        SiteTester.Posts.Create.GoTo();
	    }
	}
}