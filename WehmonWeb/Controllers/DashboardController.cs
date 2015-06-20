using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using WebDAL;
using WehmonWeb.Models;
using WebLogic.Helpers;

namespace WehmonWeb.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Company/
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DisplayShifts(int pageNumber = 1, int userId = -1, string startDate = "", string endDate = "")
        {
            int pageSize = 15;
            var model = new ShiftModel();
            ViewBag.CurrentPage = pageNumber;
            ViewBag.CurrentUser = userId;
            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            DateTime startD = DateTime.Now;
            DateTime endD = DateTime.Now;
            if (!String.IsNullOrEmpty(startDate) && !String.IsNullOrEmpty(endDate) && userId != -1)
            {
                startD = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                endD = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            using (var context = new vehmonEntities())
            {
                var users = new List<KeyValuePair<int, string>>
                {
                    new KeyValuePair<int, string>(-1, "All")
                };
                var currentCompany = context.users.Single(x => x.username == HttpContext.User.Identity.Name).company;
                users.AddRange(
                    currentCompany.users.Select(x => new KeyValuePair<int, string>(x.userID, x.username)).ToList());
                ViewBag.Users = users;

                var query = context.timetrackings.OrderByDescending(x => x.clockInTime)
                    .Where(x => x.clockInTime != null);
                if (userId != -1)
                {
                    query = query.Where(x => x.userID == userId);
                }
                if (!String.IsNullOrEmpty(startDate) && !String.IsNullOrEmpty(endDate) && userId != -1)
                {
                    query = query.Where(x => x.clockInTime > startD && x.clockOutTime!=null && x.clockOutTime < endD);
                   
                }
                if (userId > 0)
                {
                    var list = query.Where(x => x.clockOutTime != null).ToList();
                    model.TotalHours = list.Sum(x =>
                    {
                        var hours = (decimal)x.clockOutTime.Value.Subtract(x.clockInTime).TotalHours;
                        return hours;
                    });
                }
                ViewBag.PageCount = (int)Math.Ceiling((double)(query.Count() / pageSize));
                query = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                model.EntryModels = query.ToList().Select(x =>
                {
                    var shiftModel = new ShiftEntryModel()
                    {
                        ClockInDate = x.clockInTime,
                        ClockOutDate = x.clockOutTime,
                        ShiftId = x.timeTrackingID,
                        User = new KeyValuePair<int, string>(x.userID, x.user.username)
                    };
                    if (x.clockOutTime != null)
                    {
                        shiftModel.HoursWorked = x.clockOutTime.Value.Subtract(x.clockInTime).TotalHours;
                    }
                    return shiftModel;
                }
            ).
            ToList();
            }
            return View(model);
        }

        public ActionResult CreateLeaveRequest(int leaveUserId, int leaveTypeId, string startDate, string endDate, int pageNumber = 1, bool showAll = false, int userId = -1)
        {
            using (var context = new vehmonEntities())
            {
                DateTime startD;
                DateTime endD;
                DateTime.TryParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out startD);
                DateTime.TryParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out endD);
                var absenceType = context.absencetypes.Single(x => x.absenceTypeID == leaveTypeId);
                var leave = new userabsence
                {
                    absencetype = absenceType,
                    approved = false,
                    fromDate = startD,
                    toDate = endD,
                };
                var user = context.users.Single(x => x.userID == leaveUserId);
                user.userabsences.Add(leave);
                context.SaveChanges();
            }
            return RedirectToAction("DisplayLeave", new { pageNumber, showAll, userId });
        }

        public ActionResult ApproveLeave(int leaveId, int pageNumber = 1, bool showAll = false, int userId = -1)
        {
            using (var context = new vehmonEntities())
            {
                var leave = context.userabsences.Single(x => x.userAbsenseID == leaveId);
                leave.approved = true;
                context.SaveChanges();
            }
            return RedirectToAction("DisplayLeave", new { pageNumber, showAll, userId });
        }

        public ActionResult RejectLeave(int leaveId, int pageNumber = 1, bool showAll = false, int userId = -1)
        {
            using (var context = new vehmonEntities())
            {
                var leave = context.userabsences.Single(x => x.userAbsenseID == leaveId);
                context.userabsences.Remove(leave);
                context.SaveChanges();
            }
            return RedirectToAction("DisplayLeave", new { pageNumber, showAll, userId });
        }

        public ActionResult DisplayLeave(int pageNumber = 1, bool showAll = false, int userId = -1)
        {
            int pageSize = 15;
            List<LeaveModel> models = new List<LeaveModel>();
            ViewBag.CurrentPage = pageNumber;
            ViewBag.showAll = showAll;
            ViewBag.CurrentUser = userId;
            using (var context = new vehmonEntities())
            {
                var currentCompany = context.users.Single(x => x.username == HttpContext.User.Identity.Name).company;
                var users = new List<KeyValuePair<int, string>>
                {
                    new KeyValuePair<int, string>(-1, "All")
                };
                users.AddRange(currentCompany.users.Select(x => new KeyValuePair<int, string>(x.userID, x.username)).ToList());
                ViewBag.Users = users;
                ViewBag.LeaveTypes = context.absencetypes.ToList().Select(x => new KeyValuePair<int, string>(x.absenceTypeID, x.absenceTypeCode)).ToList();
                IEnumerable<userabsence> leaves = currentCompany.users.Where(x => x.isApproved)
                        .SelectMany(x => x.userabsences).OrderBy(x => x.fromDate);
                if (userId != -1)
                {
                    leaves = leaves.Where(x => x.userId == userId);
                }
                if (showAll)
                {
                    ViewBag.PageCount = (int)Math.Ceiling((double)(leaves.Count() / pageSize));

                    leaves = leaves
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                }
                else
                {
                    leaves = leaves.Where(x => !x.approved);
                    ViewBag.PageCount = (int)Math.Ceiling((double)(leaves.Count(x => !x.approved) / pageSize));

                    leaves = leaves
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
                }
                models = leaves.Select(x => new LeaveModel
                {
                    UserName = x.user.username,
                    ToDate = x.toDate,
                    FromDate = x.fromDate,
                    IsApproved = x.approved,
                    UserId = x.userId,
                    LeaveId = x.userAbsenseID,
                    LeaveType = x.absencetype.absenceTypeCode
                }).ToList();
            }
            return View("LeaveView", models);
        }

        [Authorize]
        public ActionResult Company()
        {
            var model = new List<CompanyModel>();
            using (var context = new vehmonEntities())
            {
                model = context.companies.Select(x => new CompanyModel
                {
                    CompanyId = x.companyID,
                    CompanyName = x.companyName,
                    CompanyUserCount = x.userCount,
                    IsCompanyActive = x.isActive
                }).ToList();
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateUpdateCompany(int companyId, string companyName, int companyUserCount, string isEnabled)
        {
            using (var context = new vehmonEntities())
            {
                bool companyEnabled = isEnabled != null;
                if (companyId == -1)
                {
                    var company = new company
                    {
                        companyName = companyName,
                        dateAdded = DateTime.Now,
                        isActive = companyEnabled,
                        userCount = companyUserCount,
                    };
                    context.companies.Add(company);
                    var passwordHaser = new PasswordHasher();
                    string saltValue = "";
                    var passwordHash = passwordHaser.HashPassword("Password", ref saltValue, new HMACMD5());
                    var adminUser = new user
                    {
                        company = company,
                        surname = "",
                        password = passwordHash,
                        passwordSalt = saltValue,
                        firstname = "",
                        identificationNumber = "",
                        username = companyName + "-Admin",
                        created = DateTime.Now,
                        employerNumber = "",
                        isApproved = true,
                        isLockedOut = "False",
                        lastActivityDate = DateTime.Now,
                        lastLoginDate = DateTime.Now,
                        title = "",
                        updated = DateTime.Now,
                        cellNumber = "",
                        emailAddress = "admin@" + companyName + ".com",
                        lastPasswordChange = DateTime.Now,
                    };
                    company.users.Add(adminUser);
                }
                else
                {
                    var company = context.companies.FirstOrDefault(x => x.companyID == companyId);
                    if (company == null)
                    {
                        ViewBag.Error = "Can not find company";
                        return RedirectToAction("Company");
                    }
                    company.companyName = companyName;
                    company.userCount = companyUserCount;
                    company.isActive = companyEnabled;
                }
                context.SaveChanges();
            }
            return RedirectToAction("Company");
        }

        [Authorize]
        public ActionResult UserManagement()
        {
            var model = new UserScreenModel();
            using (var context = new vehmonEntities())
            {
                var currentCompany = context.users.Single(x => x.username == HttpContext.User.Identity.Name).company;
                model.CompanyName = currentCompany.companyName;
                model.AllowedUserCount = currentCompany.userCount;
                model.Users = currentCompany.users.Where(x => x.isApproved).ToList().Select(x => new UserClassModel
                {
                    UserID = x.userID,
                    CellNumber = x.cellNumber,
                    EMail = x.emailAddress,
                    EmployerNumber = x.employerNumber,
                    Firstname = x.firstname,
                    IdentificationNumber = x.identificationNumber,
                    Surname = x.surname,
                    Title = x.title,
                    UserName = x.username
                }).ToList();
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateUpdateUser(String userId, String userName, String firstName, String surname, String email, String idNumber, String password)
        {
            using (var context = new vehmonEntities())
            {
                var currentCompany = context.users.Single(x => x.username == HttpContext.User.Identity.Name).company;
                var passwordHaser = new PasswordHasher();
                string saltValue = "";
                var passwordHash = passwordHaser.HashPassword(password, ref saltValue, new HMACMD5());
                if (userId == "-1")
                {
                    if (context.users.Any(x => x.username == userName))
                    {
                        TempData["Error"] = "Username " + userName + " allready exists in the DB.";
                        return RedirectToAction("UserManagement");
                    }
                    if (!(currentCompany.userCount > currentCompany.users.Count(x => x.isApproved)))
                    {
                        TempData["Error"] = "Company reached max user count.";
                        return RedirectToAction("UserManagement");
                    }

                    var newUser = new user
                    {
                        emailAddress = email,
                        username = userName,
                        firstname = firstName,
                        surname = surname,
                        identificationNumber = idNumber,
                        created = DateTime.Now,
                        employerNumber = "",
                        isApproved = true,
                        isLockedOut = "false",
                        failedPasswordAttemptCount = 0,
                        lastActivityDate = DateTime.Now,
                        lastLoginDate = DateTime.Now,
                        lastPasswordChange = DateTime.Now,
                        password = passwordHash,
                        passwordSalt = saltValue,
                        title = "MR",
                        updated = DateTime.Now,

                    };
                    currentCompany.users.Add(newUser);

                }
                else
                {
                    int userIdInt = int.Parse(userId);
                    var user = context.users.Single(x => x.userID == userIdInt);
                    user.emailAddress = email;
                    user.username = userName;
                    user.firstname = firstName;
                    user.surname = surname;
                    user.identificationNumber = idNumber;
                    user.created = DateTime.Now;
                    user.employerNumber = "";
                    user.isApproved = true;
                    user.isLockedOut = "false";
                    user.failedPasswordAttemptCount = 0;
                    user.lastActivityDate = DateTime.Now;
                    user.lastLoginDate = DateTime.Now;
                    user.lastPasswordChange = DateTime.Now;
                    user.password = passwordHash;
                    if (!String.IsNullOrEmpty(password))
                        user.passwordSalt = saltValue;
                    user.title = "MR";
                    user.updated = DateTime.Now;
                }
                context.SaveChanges();
            }

            return RedirectToAction("UserManagement");
        }

        public ActionResult DeleteUser(int userId)
        {
            using (var context = new vehmonEntities())
            {
                var users = context.users.Single(x => x.userID == userId);
                users.isApproved = false;
                context.SaveChanges();
            }
            return RedirectToAction("UserManagement");
        }

        public ActionResult MapsDashboard(int shiftId = -1)
        {
            ViewBag.RouteId = -1;
            if (shiftId != -1)
            {
                using (var context = new vehmonEntities())
                {
                    var timeTracking = context.timetrackings.Single(x => x.timeTrackingID == shiftId);
                    var route = timeTracking.routes.FirstOrDefault();
                    if (route != null)
                    {
                        ViewBag.RouteId = route.routeID;
                    }
                }
            }
            return View();
        }

    }
}
