using System.Runtime.Serialization;

namespace Logic.Contracts.TimeManagement
{
    /// <summary>
    /// Represents the state of an attempt to log a shift
    /// </summary>
    [DataContract]
    public class ShiftResponse
    {
        [DataMember]
        public ShiftLogState LogState { get; set; }
        [DataMember]
        public int ShiftId { get; set; }
    }

    [DataContract(Name = "ShiftLogState")]
    public enum ShiftLogState
    {
        [EnumMember(Value = "NotInShift")]
        NotInShift,
        [EnumMember(Value = "AllreadyInShift")]
        AllreadyInShift,
        [EnumMember(Value = "ShiftDoesNotExist")]
        ShiftDoesNotExist,
        [EnumMember(Value = "InvalidToken")]
        InvalidToken,
        [EnumMember(Value = "Success")]
        Success
    }
}
