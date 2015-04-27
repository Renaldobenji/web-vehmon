using System;
using System.Runtime.Serialization;

namespace Logic.Contracts.Messaging
{
    [DataContract]
    public class MessageResponse
    {
        [DataMember]
        public int ConversationId { get; set; }

        [DataMember]
        public SendMessageStatus MessageStatus { get; set; }

        [DataMember]
        public String Sender { get; set; }

        [DataMember]
        public String Message { get; set; }

        [DataMember]
        public bool HasReceived { get; set; }
    }

    [DataContract]
    public enum SendMessageStatus
    {
        [EnumMember(Value = "Successfull")]
        Successfull,
        [EnumMember(Value = "Failed")]
        Failed,
        [EnumMember(Value = "UserNotPartOfConversation")]
        UserNotPartOfConversation,
        [EnumMember(Value = "UserNotFound")]
        UserNotFound,
    }
}
