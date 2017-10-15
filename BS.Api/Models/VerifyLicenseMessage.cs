using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Api.Models
{
    public class VerifyLicenseMessage
    {
        public string Key { get; set; }

        public string License { get; set; }

        public string LicenseId { get; set; }
    }
}