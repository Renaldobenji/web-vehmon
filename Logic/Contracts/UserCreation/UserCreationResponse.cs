using System.Runtime.Serialization;

namespace Logic.Contracts.UserCreation
{
    /// <summary>
    /// Represents the state of a user creation request
    /// </summary>
    [DataContract]
    public class UserCreationResponse
    {
        [DataMember]
        public bool IsSuccess { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public int NewUserId { get; set; }
    }

    [DataContract]
    public enum UserCreationState
    {
        [EnumMember(Value = "Success")]
        Success,
        [EnumMember(Value = "PasswordInvalid")]
        PasswordInvalid,
        [EnumMember(Value = "UserAllreadyExists")]
        UserAllreadyExists,
    }
}