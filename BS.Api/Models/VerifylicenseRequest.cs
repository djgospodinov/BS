using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Api.Models
{
    public class VerifyLicenseRequest
    {
        /// <summary>
        /// the activation key needed from the mirage, e.g. comptuer id or specific logged user
        /// </summary>
        public string ActivationKey { get; set; }
        /// <summary>
        /// comptuter name where mirage is used
        /// </summary>
        public string ComputerName { get; set; }
    }
}