using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using User.Feedback.Common;

namespace User.Feedback.Client.BusinessObjects
{
    public interface IUserFeedbackManager
    {
        void TellUserFeedback(UserFeedback userFeedback);

        void TellBatchOfUserFeedbacks(UserFeedback userFeedback, int count);

        Task<List<UserFeedback>> AskUserFeedbackCollection();

        void RaiseUserFeedbackUpdate(UserFeedback userFeedback);

        event EventHandler<UserFeedback> UserFeedbackUpdated;
    }
}