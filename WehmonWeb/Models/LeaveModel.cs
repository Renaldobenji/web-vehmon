using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WehmonWeb.Models
{
    public class LeaveModel
    {
        public string UserName { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime FromDate { get; set; }

        public string LeaveType { get; set; }
    }
}