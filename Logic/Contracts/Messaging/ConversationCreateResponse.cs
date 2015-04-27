using System;
using System.Runtime.Serialization;

namespace Logic.Contracts.Messaging
{
    [DataContract]
    public class ConversationCreateResponse
    {
        [DataMember]
        public int ConversationId { get; set; }

        [DataMember]
        public ConversationCreateResponseStatus CreateStatus { get; set; }

    }

    [DataContract]
    public enum ConversationCreateResponseStatus
    {
        [EnumMember(Value = "Successfull")]
        Successfull,
        [EnumMember(Value = "Failed")]
        Failed
    }
}
