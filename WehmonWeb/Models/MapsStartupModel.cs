using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WehmonWeb.Models
{
    public class MapsStartupModel
    {
        public List<TruckModel> Trucks { get; set; }
    }

    public class TruckModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}