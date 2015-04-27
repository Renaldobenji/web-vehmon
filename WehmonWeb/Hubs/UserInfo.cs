using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WehmonWeb.Hubs
{
    public class UserInfo
    {
        public string ConnectionId { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public string UserGroup { get; set; }

        public string freeflag { get; set; }

        public string tpflag { get; set; }
    }
}
