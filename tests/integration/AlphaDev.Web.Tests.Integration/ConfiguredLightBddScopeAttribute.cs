using AlphaDev.Web.Tests.Integration.Features;
using JetBrains.Annotations;
using LightBDD.Core.Configuration;
using LightBDD.Framework.Notification;
using LightBDD.Framework.Notification.Configuration;
using LightBDD.XUnit2;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration
{
    public class ConfiguredLightBddScopeAttribute : LightBddScopeAttribute
    {
        protected override void OnConfigure([NotNull] LightBddConfiguration configuration)
        {
            var notifierConfig = configuration.Get<ScenarioProgressNotifierConfiguration>();

            notifierConfig
                .AppendNotifierProviders<WebFeatureFixture>(tester => new DelegatingScenarioProgressNotifier(new
                        SeleniumNotifier(tester.SiteTester.Driver as ITakesScreenshot),
                    new LogNotifier(tester)));
        }
    }
}