
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
    
public partial class absencetype
{

    public absencetype()
    {

        this.userabsences = new HashSet<userabsence>();

    }


    public int absenceTypeID { get; set; }

    public string absenceTypeCode { get; set; }

    public string absenceTypeDescription { get; set; }



    public virtual ICollection<userabsence> userabsences { get; set; }

}

}
