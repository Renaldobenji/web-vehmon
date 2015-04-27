using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WehmonWeb.Models
{
    public class CompanyModel
    {
        public int CompanyId { get; set; }
        [Display(Name = "Company Name")]
        public String CompanyName { get; set; }
        [Display(Name="Number Of Users")]
        public int CompanyUserCount { get; set; }
        [Display(Name = "Company Active")]
        public bool IsCompanyActive { get; set; }
    }
}