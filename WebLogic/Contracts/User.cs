using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLogic.Contracts
{
    public class User
    {
        public string UserName;
        public string Title;
        public string FirstName;
        public int UserId;
        public string EmailAddress;
        public string Surname;
        public DateTime? LastActivityDate;
        public DateTime? LastPasswordChangeDate;
        public DateTime Created;
    }
}
