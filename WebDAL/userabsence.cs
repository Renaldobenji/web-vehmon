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
    
    public partial class userabsence
    {
        public int userAbsenseID { get; set; }
        public int userId { get; set; }
        public int absenseTypeID { get; set; }
        public System.DateTime fromDate { get; set; }
        public System.DateTime toDate { get; set; }
    
        public virtual absencetype absencetype { get; set; }
        public virtual user user { get; set; }
    }
}
