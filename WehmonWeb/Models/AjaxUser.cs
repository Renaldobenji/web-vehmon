using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WehmonWeb.Models
{
    public class AjaxUser
    {
        public int ConversationId { get; set; }
        public List<ConversationUser> Users { get; set; }
    }
}