//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class coord
    {
        public int coordID { get; set; }
        public int routeID { get; set; }
        public System.DateTime time { get; set; }
        public Nullable<decimal> lat { get; set; }
        public Nullable<decimal> lng { get; set; }
    
        public virtual route route { get; set; }
    }
}