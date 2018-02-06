using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Admin.Web.Models
{
    public class LicenseLogFilterGridModel : FilterGridModelBase
    {
        public bool? IsDemo { get; set; }
        public string ChangedBy { get; set; }
    }
}