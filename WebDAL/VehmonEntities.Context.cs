﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class vehmonEntities : DbContext
    {
        public vehmonEntities()
            : base("name=vehmonEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<absencetype> absencetypes { get; set; }
        public DbSet<auditentry> auditentries { get; set; }
        public DbSet<auditentrytype> auditentrytypes { get; set; }
        public DbSet<authenticationtoken> authenticationtokens { get; set; }
        public DbSet<conversation> conversations { get; set; }
        public DbSet<coord> coords { get; set; }
        public DbSet<devicecoordinate> devicecoordinates { get; set; }
        public DbSet<groupmembership> groupmemberships { get; set; }
        public DbSet<grouprolemapping> grouprolemappings { get; set; }
        public DbSet<group> groups { get; set; }
        public DbSet<message> messages { get; set; }
        public DbSet<role> roles { get; set; }
        public DbSet<route> routes { get; set; }
        public DbSet<timetracking> timetrackings { get; set; }
        public DbSet<userabsence> userabsences { get; set; }
        public DbSet<userconversation> userconversations { get; set; }
        public DbSet<usermessagereceipt> usermessagereceipts { get; set; }
        public DbSet<userrolemapping> userrolemappings { get; set; }
        public DbSet<user> users { get; set; }
        public DbSet<company> companies { get; set; }
    }
}