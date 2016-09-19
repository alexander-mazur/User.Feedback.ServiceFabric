using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

using User.Feedback.Client.ActorEvents;
using User.Feedback.ProcessorActor.Interfaces;
using User.Feedback.Common;
using User.Feedback.PersistenceActor.Interfaces;

namespace User.Feedback.Client.BusinessObjects
{
    public class UserFeedbackManager : IUserFeedbackManager
    {
        private readonly Stopwatch _stopwatch;
        private string _lastUserFeedbackMessage = string.Empty;

        private readonly IProcessorActor _processorActor;
        private readonly IPersistenceActor _persistenceActor;

        public UserFeedbackManager()
        {
            _stopwatch = new Stopwatch();

            _processorActor = ActorProxy.Create<IProcessorActor>(new ActorId("User.Feedback.ProcessorActor"), "fabric:/User.Feedback.SF");
            _persistenceActor = ActorProxy.Create<IPersistenceActor>(new ActorId("User.Feedback.PersistenceActor"), "fabric:/User.Feedback.SF");

            _processorActor.SubscribeAsync<IProcessorActorEvents>(new ProcessorActorEventHandler(this));
        }

        public static IUserFeedbackManager Instance { get; } = new UserFeedbackManager();

        public void TellUserFeedback(UserFeedback userFeedback)
        {
            _processorActor.TellUserFeedbackAsync(userFeedback);
        }

        public void TellBatchOfUserFeedbacks(UserFeedback userFeedback, int count)
        {
            _stopwatch.Reset();
            _stopwatch.Start();

            for (var index = 0; index < count; index++)
            {
                var newUserFeedback = new UserFeedback($"{index + 1}:{userFeedback.Message}", userFeedback.Created);

                if (index == count - 1)
                {
                    _lastUserFeedbackMessage = newUserFeedback.Message;
                }

                TellUserFeedback(newUserFeedback);
            }
        }

        public Task<List<UserFeedback>> AskUserFeedbackCollection()
        {
            return _persistenceActor.AskUserFeedbackCollection();
        }

        public void RaiseUserFeedbackUpdate(UserFeedback userFeedback)
        {
            if (_stopwatch.IsRunning && userFeedback.Message.StartsWith(_lastUserFeedbackMessage))
            {
                _stopwatch.Stop();

                MessageBox.Show($"The process time of batch messages is {_stopwatch.ElapsedMilliseconds} ms.");
            }

            UserFeedbackUpdated?.Invoke(this, userFeedback);
        }

        public event EventHandler<UserFeedback> UserFeedbackUpdated;
    }
}