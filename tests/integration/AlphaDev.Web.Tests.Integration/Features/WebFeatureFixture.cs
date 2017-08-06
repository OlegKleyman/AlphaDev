using AlphaDev.Web.Tests.Integration.Fixtures;
using LightBDD.XUnit2;
using Xunit;
using Xunit.Abstractions;

namespace AlphaDev.Web.Tests.Integration.Features
{
    public class WebFeatureFixture : FeatureFixture, IClassFixture<SiteTester>
    {
        protected WebFeatureFixture(ITestOutputHelper output, SiteTester siteTester) : base(output)
        {
            SiteTester = siteTester;
        }

        public SiteTester SiteTester { get; }
    }
}