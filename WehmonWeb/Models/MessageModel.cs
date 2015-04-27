using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WehmonWeb.Models
{
    public class MessageModel
    {
        public DateTime Time { get; set; }
        public String Sender { get; set; }
        public String Message { get; set; }
    }
}