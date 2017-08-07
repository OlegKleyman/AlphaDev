using System.IO;
using System.Text.RegularExpressions;
using LightBDD.Core.Metadata;
using LightBDD.Core.Notification;
using LightBDD.Core.Results;
using OpenQA.Selenium;

namespace AlphaDev.Web.Tests.Integration
{
    public class SeleniumNotifier : IScenarioProgressNotifier
    {
        private static readonly Regex NameCleanser =
            new Regex($@"[{string.Join(string.Empty, Path.GetInvalidFileNameChars())}|\s]", RegexOptions.Compiled);

        private readonly ITakesScreenshot _screenshotTaker;
        private string _scenarioName;

        public SeleniumNotifier(ITakesScreenshot screenshotTaker)
        {
            _screenshotTaker = screenshotTaker;
        }

        public void NotifyScenarioStart(IScenarioInfo scenario)
        {
            _scenarioName = NameCleanser.Replace(scenario.Name.ToString(), string.Empty);
        }

        public void NotifyScenarioFinished(IScenarioResult scenario)
        {
        }

        public void NotifyStepStart(IStepInfo step)
        {
            TakeScreenshot(step, "1");
        }

        public void NotifyStepFinished(IStepResult step)
        {
            TakeScreenshot(step.Info, "2");
        }

        public void NotifyStepComment(IStepInfo step, string comment)
        {
        }

        private void TakeScreenshot(IStepInfo step, string suffix)
        {
            if (_screenshotTaker != null)
            {
                const string screenshotsDirectoryName = "sout";

                if (!Directory.Exists(screenshotsDirectoryName))
                    Directory.CreateDirectory(screenshotsDirectoryName);

                var escapedStepFileName = NameCleanser.Replace(step.Name.ToString(), string.Empty);

                _screenshotTaker.GetScreenshot()
                    ?.SaveAsFile(
                        $@"{screenshotsDirectoryName}/{_scenarioName}{step.Number}{escapedStepFileName}{suffix}.png",
                        ScreenshotImageFormat.Png);
            }
        }
    }
}