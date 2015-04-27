using System.Runtime.Serialization;

namespace Logic.Contracts.Messaging
{
    [DataContract]
    public class RemoveConversationResponse
    {
        [DataMember]
        public RemoveConversationStatus RemoveStatus { get; set; }
    }

    [DataContract]
    public enum RemoveConversationStatus
    {
        [EnumMember(Value = "Success")]
        Success,
        [EnumMember(Value = "ConversationNotFound")]
        ConversationNotFound,
        [EnumMember(Value = "UserNotInConversation")]
        UserNotInConversation
    }
}
