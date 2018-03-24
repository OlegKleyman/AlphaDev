using AlphaDev.Web.Tests.Integration.Fixtures;

namespace AlphaDev.Web.Tests.Integration.Features
{
    public class CommonSteps
    {
        public CommonSteps(SiteTester siteTester, DatabasesFixture databasesFixture)
        {
            SiteTester = siteTester;
            DatabasesFixture = databasesFixture;
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
    }
}