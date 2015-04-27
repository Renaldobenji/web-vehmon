using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WehmonWeb.Models
{
    public class ChatModel
    {
        public List<ConversationModel> ConversationModels { get; set; }

        public IEnumerable<ConversationUser> UserList { get; set; }
    }

}