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
        private readonly ITakesScreenshot _screenshotTaker;
        private IScenarioInfo _scenario;

        public SeleniumNotifier(ITakesScreenshot screenshotTaker)
        {
            _screenshotTaker = screenshotTaker;
        }

        public void NotifyScenarioStart(IScenarioInfo scenario)
        {
            _scenario = scenario;
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
                {
                    Directory.CreateDirectory(screenshotsDirectoryName);
                }

                var escapedStepFileName = Regex.Replace(step.Name.ToString(),
                    $@"[{string.Join(string.Empty, Path.GetInvalidFileNameChars())}|\s]", string.Empty,
                    RegexOptions.Compiled);
                _screenshotTaker.GetScreenshot()
                    ?.SaveAsFile(
                        $@"{screenshotsDirectoryName}/{_scenario.Name}{step.Number}{escapedStepFileName}{suffix}.png",
                        ScreenshotImageFormat.Png);
            }
        }
    }
}