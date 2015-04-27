using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WehmonWeb.Models
{
    public class ConversationUserModel
    {
        public IEnumerable<ConversationUser> UserList { get; set; }

        public IEnumerable<ConversationUser> ConversationUserList { get; set; }
    }
}