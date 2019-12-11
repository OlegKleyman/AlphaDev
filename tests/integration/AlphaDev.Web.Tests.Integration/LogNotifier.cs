using AlphaDev.Web.Tests.Integration.Features;
using LightBDD.Core.Metadata;
using LightBDD.Core.Notification;
using LightBDD.Core.Results;

namespace AlphaDev.Web.Tests.Integration
{
    public class LogNotifier : IScenarioProgressNotifier
    {
        private readonly WebFeatureFixture _tester;

        public LogNotifier(WebFeatureFixture tester) => _tester = tester;

        public void NotifyScenarioStart(IScenarioInfo scenario)
        {
        }

        public void NotifyScenarioFinished(IScenarioResult scenario)
        {
            WriteAndClearLog();
        }

        public void NotifyStepStart(IStepInfo step)
        {
        }

        public void NotifyStepFinished(IStepResult step)
        {
        }

        public void NotifyStepComment(IStepInfo step, string comment)
        {
        }

        private void WriteAndClearLog()
        {
            _tester.TestOutput.WriteLine(_tester.Log);
            _tester.ClearLog();
        }
    }
}