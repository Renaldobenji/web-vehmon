using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WehmonWeb.Models
{
    public class PayRunModel
    {
        public String firstname {get; set;}
        public String surname { get; set; }
        public Decimal TotalHoursWorked { get; set; }
    }
}