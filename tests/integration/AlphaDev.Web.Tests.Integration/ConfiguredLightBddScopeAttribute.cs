using System;
using AlphaDev.Web.Tests.Integration.Features;
using LightBDD.Core.Configuration;
using LightBDD.Framework.Notification;
using LightBDD.Framework.Notification.Configuration;
using LightBDD.XUnit2;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration
{
    public class ConfiguredLightBddScopeAttribute : LightBddScopeAttribute
    {
        protected override void OnConfigure(LightBddConfiguration configuration)
        {
            var notifierConfig = configuration.Get<ScenarioProgressNotifierConfiguration>();

            notifierConfig
                .UpdateNotifierProvider<FeatureFixture>(tester => new DelegatingScenarioProgressNotifier(new
                        SeleniumNotifier((tester as WebFeatureFixture)?.SiteTester?.Driver as ITakesScreenshot),
                    ParallelProgressNotifierProvider.Default.CreateScenarioProgressNotifier(Console.WriteLine,
                        tester.TestOutput.WriteLine)));
        }
    }
}