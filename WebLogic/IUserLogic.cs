using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLogic.Contracts;

namespace WebLogic
{
    public interface IUserLogic
    {
        User GetUser(String userName);
        UserValidationResult ValidateUser(string userName, string userPassword);
    }
}
