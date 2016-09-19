using System;
using System.Runtime.Serialization;

namespace User.Feedback.Common
{
    [DataContract]
    public class UserFeedback
    {
        [DataMember]
        public string Message { get; private set; }

        [DataMember]
        public DateTime Created { get; private set; }

        public UserFeedback(string message, DateTime createDateTime)
        {
            Message = message;
            Created = createDateTime;
        }
    }
}

