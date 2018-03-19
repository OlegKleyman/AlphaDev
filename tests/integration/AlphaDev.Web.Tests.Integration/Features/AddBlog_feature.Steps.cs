using System.Linq;
using AlphaDev.Web.Tests.Integration.Fixtures;
using AlphaDev.Web.Tests.Integration.Support;
using FluentAssertions;
using LightBDD.Framework;
using LightBDD.XUnit2;
using Markdig;
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

	    private void And_I_entered_markdown_content()
	    {
	        SiteTester.Posts.Create.Content = _addedBlogContent = "```\ntest\n```";
	    }

	    private void When_I_preview_the_content()
	    {
	        SiteTester.Posts.Create.TogglePreview();
	    }

	    private void Then_it_should_be_rendered_to_html()
	    {
	        SiteTester.Posts.Create.Preview.Should()
	            .BeEquivalentTo(Markdown.ToHtml(_addedBlogContent).Replace("\n", "\r\n").Trim());
	    }
	}
}