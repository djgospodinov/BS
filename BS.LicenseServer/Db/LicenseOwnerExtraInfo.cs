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
    
    public partial class LicenseOwnerExtraInfo
    {
        public int Id { get; set; }
        public int LicenseOwnerId { get; set; }
        public Nullable<int> PostCode { get; set; }
        public string RegistrationAddress { get; set; }
        public string PostAddress { get; set; }
        public string MOL { get; set; }
        public string ContactPerson { get; set; }
        public string AccountingPerson { get; set; }
        public Nullable<bool> DDSRegistration { get; set; }
    
        public virtual LicenseOwner LicenseOwner { get; set; }
    }
}
