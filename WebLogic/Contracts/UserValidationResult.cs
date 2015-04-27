using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebLogic
{
    public class UserValidationResult
    {
        public UserValidationStatus ValidationStatus;
    }

    public enum UserValidationStatus
    {
        Valid,
        PasswordIncorrect,
        UserNotFound
    }
}
