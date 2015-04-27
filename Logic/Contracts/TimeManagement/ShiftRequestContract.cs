using System;
using System.Runtime.Serialization;

namespace Logic.Contracts.TimeManagement
{
    /// <summary>
    /// Object to user for a shift request
    /// </summary>
    [DataContract]
    public class ShiftRequestContract
    {
        [DataMember]
        public Guid UserToken { get; set; }

        [DataMember]
        public decimal Lat { get; set; }
        [DataMember]
        public decimal Lng { get; set; }
        [DataMember]
        public DateTime StartTime { get; set; }
    }
}
