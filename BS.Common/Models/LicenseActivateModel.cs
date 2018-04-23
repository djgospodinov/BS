using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Common.Models
{
    public class LicenseActivateModel
    {
        public string ActivationKey { get; set; }
        public string ComputerName { get; set; }
        public string ServerName { get; set; }
    }
}
