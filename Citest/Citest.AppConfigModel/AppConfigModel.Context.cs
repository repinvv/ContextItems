﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Citest.AppConfigModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AppConfigContext : DbContext
    {
        public AppConfigContext()
            : base("name=AppConfigContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<CLIENT> CLIENT { get; set; }
        public DbSet<EP_TARGET_INFO> EP_TARGET_INFO { get; set; }
        public DbSet<AUDIT_LOG> AUDIT_LOG { get; set; }
    }
}
