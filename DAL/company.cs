
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
    
public partial class company
{

    public company()
    {

        this.users = new HashSet<user>();

    }


    public int companyID { get; set; }

    public string companyName { get; set; }

    public bool isActive { get; set; }

    public System.DateTime dateAdded { get; set; }

    public int userCount { get; set; }



    public virtual ICollection<user> users { get; set; }

}

}
