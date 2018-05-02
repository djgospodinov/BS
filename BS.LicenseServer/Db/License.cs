//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class License
    {
        public License()
        {
            this.LicenseModules = new HashSet<LicenseModule>();
            this.LicenseActivations = new HashSet<LicenseActivation>();
            this.LicenseVariables = new HashSet<LicenseVariable>();
        }
    
        public System.Guid Id { get; set; }
        public System.DateTime ValidTo { get; set; }
        public bool IsDemo { get; set; }
        public int LicenseOwnerId { get; set; }
        public Nullable<bool> Enabled { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> SubscribedTo { get; set; }
        public Nullable<byte> Type { get; set; }
        public Nullable<int> WorkstationsCount { get; set; }
        public int WorkstationCount { get; set; }
    
        public virtual ICollection<LicenseModule> LicenseModules { get; set; }
        public virtual LicenseOwner LicenseOwner { get; set; }
        public virtual ICollection<LicenseActivation> LicenseActivations { get; set; }
        public virtual ICollection<LicenseVariable> LicenseVariables { get; set; }
    }
}
