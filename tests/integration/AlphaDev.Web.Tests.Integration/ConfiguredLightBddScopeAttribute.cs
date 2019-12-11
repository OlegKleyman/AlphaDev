using System;
using AlphaDev.Web.Tests.Integration.Features;
using JetBrains.Annotations;
using LightBDD.Core.Configuration;
using LightBDD.Core.Notification;
using LightBDD.Framework.Configuration;
using LightBDD.Framework.Notification;
using LightBDD.XUnit2;
using OpenQA.Selenium;
using Optional;

namespace AlphaDev.Web.Tests.Integration
{
    public class ConfiguredLightBddScopeAttribute : LightBddScopeAttribute
    {
        protected override void OnConfigure([NotNull] LightBddConfiguration configuration)
        {
            var notifierConfig = configuration.Get<ScenarioProgressNotifierConfiguration>();
            var notifier = bool.TryParse(Environment.GetEnvironmentVariable("TAKE_SELENIUM_SCREENSHOTS"),
                                   out var takeScreenshot)
                               .SomeWhen(b => takeScreenshot)
                               .Map<Func<WebFeatureFixture, IScenarioProgressNotifier>>(b => tester => new
                                   SeleniumNotifier(tester.SiteTester.Driver as ITakesScreenshot));
            notifierConfig
                .AppendNotifierProviders<WebFeatureFixture>(tester => new DelegatingScenarioProgressNotifier(
                    notifier.ValueOr(() => _ => new EmptyNotifier())(tester),
                    new LogNotifier(tester)));
        }
    }
}