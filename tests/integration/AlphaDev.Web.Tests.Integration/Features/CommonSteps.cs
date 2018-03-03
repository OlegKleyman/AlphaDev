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
    }
}