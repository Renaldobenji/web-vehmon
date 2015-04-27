using System;
using System.Collections.Generic;
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

        public ActionResult DisplayLeave()
        {
            List<LeaveModel> models = new List<LeaveModel>();
            using (var context = new vehmonEntities())
            {
                var currentCompany = context.users.Single(x => x.username == HttpContext.User.Identity.Name).company;
                var leaves = currentCompany.users.Where(x => x.isApproved).SelectMany(x => x.userabsences);
                models = leaves.Select(x => new LeaveModel
                {
                    UserName = x.user.username,
                    ToDate = x.toDate,
                    FromDate = x.fromDate,
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

        public ActionResult MapsDashboard()
        {
            return View();
        }

    }
}
