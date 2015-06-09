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
        public String LeaveRequestType { get; set; }

        [DataMember]
        public String StartDateTime { get; set; }

        [DataMember]
        public String EndDateTime { get; set; }
    }
}
