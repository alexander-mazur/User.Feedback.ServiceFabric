using System.Threading.Tasks;

using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;

using User.Feedback.PersistenceActor.Interfaces;
using User.Feedback.ProcessorActor.Interfaces;

using User.Feedback.Common;

namespace User.Feedback.ProcessorActor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class ProcessorActor : Actor, IProcessorActor
    {
        private IPersistenceActor _persistenceActor;

        /// <summary>
        /// Initializes a new instance of ProcessorActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public ProcessorActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        /// <summary>
        /// Override this method to initialize the members, initialize state or register timers. This method is called right after the actor is activated
        /// and before any method call or reminders are dispatched on it.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task">Task</see> that represents outstanding OnActivateAsync operation.
        /// </returns>
        protected override Task OnActivateAsync()
        {
            _persistenceActor = ActorProxy.Create<IPersistenceActor>(new ActorId("User.Feedback.PersistenceActor"), "fabric:/User.Feedback.SF");

            return base.OnActivateAsync();
        }

        public Task TellUserFeedbackAsync(UserFeedback userFeedback)
        {
            return _persistenceActor.TellUserFeedbackAsync(userFeedback).ContinueWith(task =>
            {
                var processorActorEvent = GetEvent<IProcessorActorEvents>();
                processorActorEvent.UserFeedbackUpdated(new UserFeedback(userFeedback.Message + "*", userFeedback.Created));
            });
        }
    }
}
