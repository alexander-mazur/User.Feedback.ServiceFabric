using User.Feedback.Client.BusinessObjects;
using User.Feedback.Common;
using User.Feedback.ProcessorActor.Interfaces;

namespace User.Feedback.Client.ActorEvents
{
    internal class ProcessorActorEventHandler : IProcessorActorEvents
    {
        private IUserFeedbackManager UserFeedbackManager { get; }

        public ProcessorActorEventHandler(IUserFeedbackManager userFeedbackManager)
        {
            UserFeedbackManager = userFeedbackManager;
        }

        public void UserFeedbackUpdated(UserFeedback userFeedback)
        {
            UserFeedbackManager.RaiseUserFeedbackUpdate(userFeedback);
        }
    }
}