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
    
    public partial class LicenseVariable
    {
        public int Id { get; set; }
        public System.Guid LicenseId { get; set; }
        public int VariableId { get; set; }
        public string Value { get; set; }
    
        public virtual License License { get; set; }
        public virtual lu_LicenseVariables lu_LicenseVariables { get; set; }
    }
}
