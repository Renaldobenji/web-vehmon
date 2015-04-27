using System;
using System.Runtime.Serialization;

namespace Logic.Contracts.UserCreation
{
    /// <summary>
    /// Represents the state of a token generation request
    /// </summary>
    [DataContract]
    public class TokenGenerationResult
    {
        [DataMember]
        public String GeneratedToken { get; set; }

        [DataMember]
        public TokenGenerationState TokenGenerationState { get; set; }
    }

    /// <summary>
    /// The state of the request
    /// </summary>
    [DataContract (Name = "TokenGenerationState")]
    public enum TokenGenerationState
    {
        [EnumMember(Value = "UserNotFound")]
        UserNotFound,
        [EnumMember(Value = "InvalidPassword")]
        InvalidPassword,
        [EnumMember(Value = "Valid")]
        Valid
    }
}
