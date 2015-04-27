using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WehmonWeb.Models
{
    public class UserScreenModel
    {
        [Display(Name = "Company Name")]
        public String CompanyName { get; set; }
        [Display(Name = "Allowed User Count")]
        public int AllowedUserCount { get; set; }
        public List<UserClassModel> Users { get; set; }
    }

    public class UserClassModel
    {
        public int UserID { get; set; }

        [Display(Name = "User Name")]
        public String UserName { get; set; }
        [Display(Name = "Email")]
        public String EMail { get; set; }
        [Display(Name = "Title")]
        public String Title { get; set; }
        [Display(Name = "Firstname")]
        public String Firstname { get; set; }
        [Display(Name = "Surname")]
        public String Surname { get; set; }
        [Display(Name = "Employer Number")]
        public String EmployerNumber { get; set; }
        [Display(Name = "ID number")]
        public String IdentificationNumber { get; set; }
        [Display(Name = "Cellphone Number")]
        public String CellNumber { get; set; }
    }
}