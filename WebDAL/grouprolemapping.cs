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
    
    public partial class grouprolemapping
    {
        public int groupRoleMappingID { get; set; }
        public string groupCode { get; set; }
        public string roleCode { get; set; }
        public System.DateTime fromDate { get; set; }
        public Nullable<System.DateTime> toDate { get; set; }
    }
}
