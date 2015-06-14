using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Logic.Contracts.Leave
{
    [DataContract]
    public class LeaveRequestResponse
    {
        [DataMember]
        public LeaveRequestStatus RequestStatus { get; set; }

        [DataMember]
        public List<LeaveRequestContract> LeaveRequests { get; set; }

        [DataMember]
        public int AvailableBalance { get; set; }

        [DataMember]
        public int RequestId { get; set; }
    }

    [DataContract]
    public enum LeaveRequestStatus
    {
        [EnumMember(Value = "Success")]
        Success,
        [EnumMember(Value = "AllreadyLeaveForPeriod")]
        AllreadyLeaveForPeriod,
        [EnumMember(Value = "InvalidToken")]
        InvalidToken,
        [EnumMember(Value = "LeaveNotFound")]
        LeaveNotFound
    }
}
