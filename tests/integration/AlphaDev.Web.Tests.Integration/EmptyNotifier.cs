using LightBDD.Core.Metadata;
using LightBDD.Core.Notification;
using LightBDD.Core.Results;

namespace AlphaDev.Web.Tests.Integration
{
    public class EmptyNotifier : IScenarioProgressNotifier
    {
        public void NotifyScenarioStart(IScenarioInfo scenario)
        {
        }

        public void NotifyScenarioFinished(IScenarioResult scenario)
        {
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
    }
}