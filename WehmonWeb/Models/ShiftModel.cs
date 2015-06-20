using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WehmonWeb.Models
{
    public class ShiftModel
    {
        public List<ShiftEntryModel> EntryModels { get; set; }

        public decimal TotalHours { get; set; }
    }
}