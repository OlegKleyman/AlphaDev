using System;
using System.Linq;
using System.Threading.Tasks;
using AlphaDev.Web.Tests.Integration.Fixtures;
using FluentAssertions;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;
using Omego.Extensions.DbContextExtensions;
using Optional;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
	public partial class Post_feature: WebFeatureFixture
	{
	    private int? _postId;
	    private string _title;

	    private void When_I_view_it_by_id()
	    {
	        SiteTester.Posts.GoTo(_postId.ToOption()
	            .ValueOr(() => throw new InvalidOperationException("_postId was not initialized.")));
	    }

	    public Post_feature(ITestOutputHelper output, DatabaseWebServerFixture databaseWebServerFixture) : base(output, databaseWebServerFixture)
	    {
	    }

	    private void Then_it_should_load_the_post()
	    {
	        SiteTester.Posts.Title.Should().BeEquivalentTo($"{_title} - AlphaDev");
	    }

	    private void Given_there_is_a_blog_post(string title)
	    {
	        var defaultBlog = DatabaseFixture.DefaultBlog;
	        defaultBlog.Title = title;

	        DatabaseFixture.BlogContext.AddRangeAndSave(defaultBlog);

	        var blog = DatabaseFixture.BlogContext.Blogs.FirstOrDefault().SomeNotNull()
	            .ValueOr(() => throw new InvalidOperationException("No blogs found"));

	        _postId = blog.Id;
	        _title = blog.Title;
	    }

	    private CompositeStep When_I_view_a_post_by_id()
	    {
	        return CompositeStep.DefineNew()
	            .AddSteps(_ => Given_there_is_a_blog_post("testing"), _ => When_I_view_it_by_id()).Build();
	    }
	}
}