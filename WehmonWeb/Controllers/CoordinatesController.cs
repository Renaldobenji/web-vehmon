using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using WebDAL;
using WehmonWeb.Hubs;
using WehmonWeb.Models;

namespace WehmonWeb.Controllers
{
    public class CoordinatesController : Controller
    {
        //
        // GET: /Coordinates/
        public ActionResult RenderTruckDashboard()
        {
            var result = GetMapsStartupModel();
            return PartialView("_TrucksDasboard", result);
        }

        public JsonResult GetTruckCoordinates(int userId)
        {
            List<Coordinate> result = new List<Coordinate>();
            using (var context = new vehmonEntities())
            {
                var user = context.users.Single(x => x.userID == userId);
                var shift = user.timetrackings.OrderByDescending(x => x.clockInTime).FirstOrDefault();//last shift
                if (shift == null)
                {
                    result.Add(new Coordinate { Lat = -31.397, Lng = 25.644 + userId, Date = DateTime.Now });
                    result.Add(new Coordinate { Lat = -31.497, Lng = 25.644 + userId, Date = DateTime.Now });
                    result.Add(new Coordinate { Lat = -31.597, Lng = 25.644 + userId, Date = DateTime.Now });
                    result.Add(new Coordinate { Lat = -31.697, Lng = 25.644 + userId, Date = DateTime.Now });
                    result.Add(new Coordinate { Lat = -31.797, Lng = 25.644 + userId, Date = DateTime.Now });
                    result.Add(new Coordinate { Lat = -31.897, Lng = 25.644 + userId, Date = DateTime.Now });
                    result.Add(new Coordinate { Lat = -31.997, Lng = 25.644 + userId, Date = DateTime.Now });
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                result = shift.routes.Single().coords.OrderByDescending(c => c.time).Take(7).ToList().Select(x => new Coordinate
                {
                    Lat = Convert.ToDouble(x.lat.Value),
                    Lng = Convert.ToDouble(x.lng.Value),
                    Date = x.time
                }).ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetStartupModel()
        {
            var result = GetMapsStartupModel();

            return Json(result);
        }

        private MapsStartupModel GetMapsStartupModel()
        {
            var result = new MapsStartupModel();
            using (var context = new vehmonEntities())
            {
                var currentCompany = context.users.Single(x => x.username == HttpContext.User.Identity.Name).company;
                result.Trucks =
                    currentCompany.users.Where(
                        x => x.isApproved == true)
                        .Select(x => new TruckModel
                        {
                            UserId = x.userID,
                            UserName = x.username
                        }).ToList();
            }
            return result;
        }

        public JsonResult Log(Guid token, string coordinates)
        {
            using (var context = new vehmonEntities())
            {
                var coordinateList = new List<Coordinate>();
                var splitVals = coordinates.Split(","[0]);
                for (var i = 0; i < splitVals.Length; i += 3)
                {
                    coordinateList.Add(new Coordinate { Lat = double.Parse(splitVals[i]), Lng = double.Parse(splitVals[i + 1]) });
                }
                var tokenFound = context.authenticationtokens.FirstOrDefault(x => x.authenticationTokenValue == token);
                if (tokenFound == null)
                    return Json(new { success = false, error = "Token not found" });
                var user = tokenFound.user;

                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                string companyName = user.company.companyName;
                hubContext.Clients.All.UpdateCoordinates(user.userID, coordinateList);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetUserGuid(int userId)
        {
            string response;
            using (var context = new vehmonEntities())
            {
                var user = context.users.FirstOrDefault(x => x.userID == userId);

                var token = user.authenticationtokens.FirstOrDefault();
                if (token != null)
                {
                    token.ipAddress = "";
                    token.issueDate = DateTime.Now;
                    token.lastActivityDate = DateTime.Now;
                }
                else
                {
                    token = new authenticationtoken
                    {
                        authenticationTokenValue = Guid.NewGuid(),
                        ipAddress = "",
                        issueDate = DateTime.Now,
                        lastActivityDate = DateTime.Now,
                        user = user
                    };
                    context.authenticationtokens.Add(token);
                }
                response = token.authenticationTokenValue.ToString();
                context.SaveChanges();
            }
            return Json(new { success = true, token = response }, JsonRequestBehavior.AllowGet);
        }



    }
}
