using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WebDAL;
using WebLogic.Contracts;
using WebLogic.Helpers;

namespace WebLogic
{
    public class UserLogic : IUserLogic
    {
        private PasswordHasher _passwordHasher;
        public UserLogic()
        {
            _passwordHasher = new PasswordHasher();
        }

        public UserValidationResult ValidateUser(string userName, string userPassword)
        {
            UserValidationResult result = new UserValidationResult
            {
                ValidationStatus = UserValidationStatus.Valid
            };

            using (var context = new vehmonEntities())
            {
                var user = context.users.FirstOrDefault(x => x.username == userName);
                if (user == null)
                {
                    result.ValidationStatus = UserValidationStatus.UserNotFound;
                }
                else if (!_passwordHasher.ValidatePassword(userPassword, user.passwordSalt, user.password))
                {
                    result.ValidationStatus = UserValidationStatus.PasswordIncorrect;
                }
                return result;
            }
        }

        public User GetUser(String userName)
        {
            using (var context = new vehmonEntities())
            {
                var user = context.users.FirstOrDefault(x => x.username == userName);
                if (user == null)
                    return null;
                return new User
                {
                    UserName = user.username,
                    Title = user.title,
                    FirstName = user.firstname,
                    UserId = user.userID,
                    EmailAddress = user.emailAddress,
                    Surname = user.surname,
                    LastActivityDate = user.lastActivityDate,
                    LastPasswordChangeDate = user.lastPasswordChange,
                    Created = user.created
                };
            }
        }
    }
}
