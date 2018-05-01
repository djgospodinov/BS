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
    
    public partial class LicenseOwner
    {
        public LicenseOwner()
        {
            this.Licenses = new HashSet<License>();
            this.LicenseOwnerExtraInfoes1 = new HashSet<LicenseOwnerExtraInfo1>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsCompany { get; set; }
        public string ContactPerson { get; set; }
        public string CompanyId { get; set; }
        public string UserId { get; set; }
        public string EGN { get; set; }
        public string RegNom { get; set; }
        public Nullable<bool> IsVIP { get; set; }
        public string VipComment { get; set; }
    
        public virtual ICollection<License> Licenses { get; set; }
        public virtual ICollection<LicenseOwnerExtraInfo1> LicenseOwnerExtraInfoes1 { get; set; }
    }
}
