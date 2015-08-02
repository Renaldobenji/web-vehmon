using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDAL;
using WehmonWeb.Models;

namespace WehmonWeb.Controllers
{
    public class ReportsController : Controller
    {
        //
        // GET: /Reports/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PayRun(int userId = -1, string startDate = "", string endDate = "")
        {
            ViewBag.CurrentUser = userId;
            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            List<PayRunModel> results = new List<PayRunModel>();
            using (var context = new vehmonEntities())
            {

                var users = new List<KeyValuePair<int, string>>
                {
                    new KeyValuePair<int, string>(-1, "All")
                };
                var currentCompany = context.users.Single(x => x.username == HttpContext.User.Identity.Name).company;
                users.AddRange(currentCompany.users.Select(x => new KeyValuePair<int, string>(x.userID, x.username)).ToList());
                ViewBag.Users = users;
                
                if (!String.IsNullOrEmpty(startDate) && !String.IsNullOrEmpty(endDate))
                {
                    String userIDList;
                    if (userId != -1)
                    {
                        userIDList = userId.ToString();
                    }
                    else
                    {
                        userIDList = String.Join(",", users.Select(x => x.Key).ToList());
                    }
                    results = context.ExecuteStoredProcedure<PayRunModel>("sps_CalculateTotalHoursWorked", new Object[3] { userIDList, startDate, endDate });
                }
            }
            return View(results);
        }
    }
}
