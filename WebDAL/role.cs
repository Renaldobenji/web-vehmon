//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class role
    {
        public role()
        {
            this.userrolemappings = new HashSet<userrolemapping>();
        }
    
        public int roleID { get; set; }
        public string roleCode { get; set; }
        public string roleName { get; set; }
        public string roleDescription { get; set; }
        public string roleStatus { get; set; }
    
        public virtual ICollection<userrolemapping> userrolemappings { get; set; }
    }
}
