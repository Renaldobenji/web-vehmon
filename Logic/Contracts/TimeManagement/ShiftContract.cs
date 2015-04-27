using System;
using System.Runtime.Serialization;

namespace Logic.Contracts.TimeManagement
{
    /// <summary>
    /// Represents the state of an attempt to log a shift
    /// </summary>
    [DataContract]
    public class ShiftContract
    {
        [DataMember]
        public int ShiftId { get; set; }
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public DateTime EndTime { get; set; }

    }
}
