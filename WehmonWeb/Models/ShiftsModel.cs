using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WehmonWeb.Models
{
    public class ShiftsModel
    {
        public List<ShiftUsers> ShiftUserses { get; set; }

        public object ToTreeJson()
        {
            return ShiftUserses.Select(x => x.ToTreeObj()).ToArray();
        }

        public int selectedShift { get; set; }
    }

    public class ShiftUsers
    {
        public String UserName { get; set; }
        public List<UserShift> UserShifts { get; set; }

        public object ToTreeObj()
        {
            return new { text = UserName, icon = "glyphicon glyphicon-user", selectable = false, nodes = UserShifts.Select(x => x.ToTreeObj()).ToArray(), state = new { expanded = false } };
        }
    }

    public class UserShift
    {
        public String DisplayText { get; set; }
        public int RouteId { get; set; }

        public object ToTreeObj()//, routeId=RouteId+"" 
        {
            return new { text = DisplayText, selectable = true, icon = "glyphicon glyphicon-road", routeId = RouteId + "" };
        }
    }
}