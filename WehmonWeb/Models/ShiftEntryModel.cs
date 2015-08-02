using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WehmonWeb.Models
{
    public class ShiftEntryModel
    {
        public KeyValuePair<int, string> User { get; set; }

        public DateTime ClockInDate { get; set; }

        public DateTime? ClockOutDate { get; set; }

        public double HoursWorked { get; set; }

        public String StartCOORDS { get; set; }

        public String EndCOORDS { get; set; }

        public int ShiftId { get; set; }
    }
}