using System.Linq;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Web.Tests.Integration.Fixtures;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    [UsedImplicitly]
    public partial class DeleteBlog_feature : WebFeatureFixture
    {
        public DeleteBlog_feature([NotNull] ITestOutputHelper output,
            DatabaseWebServerFixture databaseWebServerFixture) : base(
            output, databaseWebServerFixture)
        {
            output.WriteLine("Given I have logged in");
            CommonSteps.Given_I_have_logged_in();
        }

        private void When_I_click_the_delete_symbol()
        {
            SiteTester.HomePage.DeleteBlog();
        }

        private void Then_it_should_be_deleted()
        {
            var addedBlog = (Blog) CommonSteps.Data["AddedBlog"];
            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.Blogs.Where(blog => blog.Id == addedBlog.Id)
                            .Should()
                            .BeEmpty();
        }

        private void Then_should_be_directed_to_the_posts_page()
        {
            SiteTester.Driver.Url.Should().BeEquivalentTo(SiteTester.Posts.BaseUrl.AbsoluteUri.Trim('/'));
        }
    }
}