using System;
using System.Runtime.Serialization;

namespace Logic.Contracts.UserCreation
{
    /// <summary>
    /// Contract for the UserTokenValidation class
    /// </summary>
    [DataContract]
    public class UserTokenValidationResponse
    {
        [DataMember]
        public String UserTokenState { get; set; }
    }

    /// <summary>
    /// Represents the state of a token
    /// </summary>
    [DataContract]
    public enum UserTokenValidationState
    {
        [EnumMember(Value = "Valid")]
        Valid,
        [EnumMember(Value = "Invalid")]
        Invalid,
        [EnumMember(Value = "Timedout")]
        Timedout,
        [EnumMember(Value = "NotInRole")]
        NotInRole
    }
}
