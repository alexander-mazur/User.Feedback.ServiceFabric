using Microsoft.ServiceFabric.Actors;

using User.Feedback.Common;

namespace User.Feedback.ProcessorActor.Interfaces
{
    public interface IProcessorActorEvents : IActorEvents
    {
        void UserFeedbackUpdated(UserFeedback userFeedback);
    }
}
