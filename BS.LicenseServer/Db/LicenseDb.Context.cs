﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BS.LicenseServer.Db
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LicenseDbEntities : DbContext
    {
        public LicenseDbEntities()
            : base("name=LicenseDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<LicenseModule> LicenseModules { get; set; }
        public virtual DbSet<LicenseOwner> LicenseOwners { get; set; }
        public virtual DbSet<License> Licenses { get; set; }
        public virtual DbSet<lu_LicenseModules> lu_LicenseModules { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<lu_ActivationTypes> lu_ActivationTypes { get; set; }
    }
}
