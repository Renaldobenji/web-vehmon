using System;
using System.Runtime.Serialization;
using Logic.Contracts.TimeManagement;

namespace Logic.Contracts.Leave
{
    [DataContract]
    public class LeaveRequestContract
    {
        [DataMember]
        public int LeaveRequestId { get; set; }

        [DataMember]
        public LeaveRequestTypes LeaveRequestType { get; set; }

        [DataMember]
        public DateTime StartDateTime { get; set; }

        [DataMember]
        public DateTime EndDateTime { get; set; }
    }
}
