using System.Collections.Generic;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Web.Tests.Integration.Fixtures;
using Omego.Extensions.DbContextExtensions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    public class CommonSteps
    {
        public CommonSteps(SiteTester siteTester, DatabasesFixture databasesFixture)
        {
            SiteTester = siteTester;
            DatabasesFixture = databasesFixture;
            Data = new Dictionary<string, object>();
        }

        public SiteTester SiteTester { get; }
        public DatabasesFixture DatabasesFixture { get; }

        public void Given_i_am_a_user()
        {
        }

        public void When_I_visit_the_admin_area()
        {
            SiteTester.Admin.GoTo();
        }

        public void Given_I_have_logged_in()
        {
            Login();
        }

        public void When_I_logged_in()
        {
            Login();
        }

        private void Login()
        {
            SiteTester.Login.GoTo();

            SiteTester.Login.Username = "something@something.com";
            SiteTester.Login.Password = "H3ll04321!";

            SiteTester.Login.Submit();
        }

        public void And_I_am_on_the_blog_posts_page()
        {
            var blog = (Blog)Data["AddedBlog"];
            SiteTester.Posts.GoTo(blog.Id);
        }

        public void Given_there_is_a_blog_post()
        {
            var blog = BlogContextDatabaseFixture.DefaultBlog;
            DatabasesFixture.BlogContextDatabaseFixture.BlogContext.AddRangeAndSave(blog);
            Data.Add("AddedBlog", blog);
        }

        public Dictionary<string, object> Data { get; }

        public void And_I_am_on_the_homepage()
        {
            SiteTester.HomePage.GoTo();
        }

        public void And_I_am_on_the_blog_post_page()
        {
            var blog = (Blog)Data["AddedBlog"];
            SiteTester.Posts.GoTo(blog.Id);
        }
    }
}