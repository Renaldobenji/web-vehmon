using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDAL;
using WebLogic;

namespace WehmonWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            //using (var context = new vehmonEntities())
            //{
            //    var users = context.users.ToList();

            //    foreach (var user in users)
            //    {
            //        var startDate = DateTime.Now.AddYears(-3);

            //        for (var i = 2; i < 30; i++)
            //        {
            //            var shit = new timetracking
            //            {
            //                clockInLat = (decimal) 33.33,
            //                clockOutLat = (decimal) 40.40,
            //                user = user,
            //                clockInLng = (decimal) 50.33,
            //                clockOutLng = (decimal) 60.00,
            //                clockInTime = startDate,
            //                clockOutTime = startDate.AddHours(i)
                            
            //            };
            //            startDate.AddDays(4);
            //            user.timetrackings.Add(shit);
            //            context.SaveChanges();
            //        }
            //    }

            //}
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult RenderMenu()
        {
            return PartialView("_MenuBar");
        }

        public ActionResult RenderSlider()
        {
            return PartialView("_Slider");
        }
    }
}
