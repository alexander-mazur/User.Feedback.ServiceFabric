using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.ServiceFabric.Actors;

using User.Feedback.Common;

namespace User.Feedback.PersistenceActor.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IPersistenceActor : IActor
    {
        Task TellUserFeedbackAsync(UserFeedback userFeedback);

        Task<List<UserFeedback>> AskUserFeedbackCollection();
    }
}
