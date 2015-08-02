using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDAL;
using WehmonWeb.Models;

namespace WehmonWeb.Controllers
{
    public class MapsController : Controller
    {
        //
        // GET: /Maps/

        public ActionResult Index(String coords)
        {
            ViewBag.coords = coords;

            return View(ViewBag);
        }

        public ActionResult RenderShiftTreeView(int selected = -1)
        {
            var model = new ShiftsModel();
            using (var context = new vehmonEntities())
            {
                var userName = HttpContext.User.Identity.Name;
                var currentUser = context.users.Single(x => x.username == userName);
                model.selectedShift = selected;
                var users = currentUser.company.users;
                model.ShiftUserses = new List<ShiftUsers>();
                foreach (var user in users)
                {
                    var newShift = new ShiftUsers
                    {
                        UserName = user.username,
                        UserShifts = user.timetrackings.Where(x => x.routes.Any()).Select(x => new UserShift
                        {
                            RouteId = x.routes.First().routeID,
                            DisplayText = "Shift Started At " + x.routes.First().startTime.ToString(CultureInfo.InvariantCulture)
                        }).ToList()
                    };
                    model.ShiftUserses.Add(newShift);
                }
            }
            return PartialView("_RenderShiftTreeView", model);
        }

        public JsonResult GetCoordinatesForRoute(int routeId)
        {
            using (var context = new vehmonEntities())
            {
                var route = context.routes.Single(x => x.routeID == routeId);
                return Json(new {routeId = routeId, routes = route.coords.OrderBy(x=>x.time).ToList().Select(x => new { lat = x.lat, lng = x.lng, date = x.time.ToString() }).ToArray()}, JsonRequestBehavior.AllowGet);
            }
        }

        public bool SetUpShitsForUser(int userId, string pathCSV)
        {
            using (var context = new vehmonEntities())
            {
                var user = context.users.Single(x => x.userID == userId);

                var coordinates = GetShiftsFromString(pathCSV);

                var firstCoordinate = coordinates.First();
                var lastCoordinate = coordinates.Last();

                var timeTracking = new timetracking
                {
                    clockInLat = firstCoordinate.Lat,
                    clockInLng = firstCoordinate.Lng,
                    clockInTime = DateTime.Now.Subtract(TimeSpan.FromHours(24)),
                    clockOutLat = lastCoordinate.Lat,
                    clockOutLng = lastCoordinate.Lng,
                    clockOutTime = DateTime.Now
                };

                user.timetrackings.Add(timeTracking);

                context.SaveChanges();

                var routeToAdd = new route
                {
                    clockInLat = firstCoordinate.Lat,
                    clockInLng = firstCoordinate.Lng,
                    startTime = DateTime.Now.Subtract(TimeSpan.FromHours(24)),
                    endTime = DateTime.Now
                };

                foreach (var coordinate in coordinates)
                {
                    routeToAdd.coords.Add(new coord
                    {
                        lat = coordinate.Lat,
                        lng = coordinate.Lng,
                        time = coordinate.Date
                    });
                }

                timeTracking.routes.Add(routeToAdd);

                context.SaveChanges();

                return true;
            }
        }

        private List<Coordinate> GetShiftsFromString(string coords)
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            var splitString = coords.Split(","[0]);
            for (var i = 0; i < coords.Count(f => f == ','); i = i + 2)
            {
                var lng = decimal.Parse(splitString[i + 1].Replace("0 ", ""));
                var lat = decimal.Parse(splitString[i].Replace("0 ", ""));

                var newCoord = new Coordinate
                {
                    Lat = lng,
                    Lng = lat,
                    Date = DateTime.Now.Add(TimeSpan.FromHours(i))
                };
                coordinates.Add(newCoord);
            }
            return coordinates;
        }

    }
}
