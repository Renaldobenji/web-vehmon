
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
    
public partial class conversation
{

    public conversation()
    {

        this.messages = new HashSet<message>();

        this.userconversations = new HashSet<userconversation>();

    }


    public int conversationID { get; set; }

    public string name { get; set; }

    public System.DateTime dateCreated { get; set; }



    public virtual ICollection<message> messages { get; set; }

    public virtual ICollection<userconversation> userconversations { get; set; }

}

}
