using System.Runtime.Serialization;

namespace Logic.Contracts.Messaging
{
    [DataContract]
    public class ConversationResponse
    {
        [DataMember]
        public ConversationStatus CreateStatus { get; set; }
        [DataMember]
        public int ConversationId { get; set; }
        [DataMember]
        public string ConversationName { get; set; }
    }


    [DataContract]
    public enum ConversationStatus
    {
        [EnumMember(Value = "Successfull")]
        Successfull,
        [EnumMember(Value = "Failed")]
        Failed,
       
    }
}
