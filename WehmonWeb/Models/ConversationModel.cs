using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WehmonWeb.Models
{
    public class ConversationModel
    {
        [Display(Name="Conversation Name")]
        public String ConversationName { get; set; }
        public List<MessageModel> Messages { get; set; }

        public int ID { get; set; }

        public List<ConversationUser> Users { get; set; }
    }
}