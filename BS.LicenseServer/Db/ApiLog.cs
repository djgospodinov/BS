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
    
    public partial class ApiLog
    {
        public int Id { get; set; }
        public string RequestUri { get; set; }
        public string RequestMethod { get; set; }
        public string RequestBody { get; set; }
        public string RequestIpAddress { get; set; }
        public System.DateTime RequestTimestamp { get; set; }
        public string ResponseContentBody { get; set; }
        public Nullable<int> ResponseStatusCode { get; set; }
        public Nullable<System.DateTime> ResponseTimestamp { get; set; }
        public string AbsoluteUri { get; set; }
        public string Host { get; set; }
    }
}