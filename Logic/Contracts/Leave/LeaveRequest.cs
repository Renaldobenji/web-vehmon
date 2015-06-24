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
        public String LeaveRequestType { get; set; }
    }
}
