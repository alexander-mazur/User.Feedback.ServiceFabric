using System.Threading.Tasks;

using Microsoft.ServiceFabric.Actors;

using User.Feedback.Common;

namespace User.Feedback.ProcessorActor.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IProcessorActor : IActor, IActorEventPublisher<IProcessorActorEvents>
    {
        Task TellUserFeedbackAsync(UserFeedback userFeedback);
    }
}
