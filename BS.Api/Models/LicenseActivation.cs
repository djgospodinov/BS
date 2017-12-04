using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Api.Models
{
    public class LicenseActivation
    {
        public string ComputerId { get; set; }
        public string UserId { get; set; }
        public string ComputerName { get; set; }
    }
}