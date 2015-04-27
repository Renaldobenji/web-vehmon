using System;
using System.Runtime.Serialization;

namespace Logic.Contracts.Leave
{
    [DataContract]
    public class LeaveRequest
    {
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public DateTime EndTime { get; set; }
        [DataMember]
        public LeaveRequestTypes LeaveRequestType { get; set; }
    }

    public enum LeaveRequestTypes
    {
        [EnumMember(Value = "Annual")]
        Annual,
        [EnumMember(Value = "Sick")]
        Sick,
        [EnumMember(Value = "Canceled")]
        Canceled
    }
}
