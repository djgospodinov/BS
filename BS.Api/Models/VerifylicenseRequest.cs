using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Api.Models
{
    public class VerifyLicenseRequest
    {
        public string ActivationKey { get; set; }
        public string ComputerName { get; set; }
    }
}