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
            TakeScreenshot(step, "before");
        }

        public void NotifyStepFinished(IStepResult step)
        {
            TakeScreenshot(step.Info, "after");
        }

        public void NotifyStepComment(IStepInfo step, string comment)
        {
        }

        private void TakeScreenshot(IStepInfo step, string suffix)
        {
            var replace = Regex.Replace(step.Name.ToString(),
                $@"[{string.Join(string.Empty, Path.GetInvalidFileNameChars())}]", string.Empty, RegexOptions.Compiled);
            _screenshotTaker?.GetScreenshot()
                ?.SaveAsFile($"{_scenario.Name}{replace}{step.Number}{suffix}.png",
                    ScreenshotImageFormat.Png);
        }
    }
}