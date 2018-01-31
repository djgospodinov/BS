using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Admin.Web.Models
{
    public class ApiLogFilterGridModel : FilterGridModelBase
    {
        public string RequestUri { get; set; }
        public string AbsoluteUri { get; set; }
        public string RequestMethod { get; set; }
        public string RequestIpAddress { get; set; }
        public int? ResponseStatusCode { get; set; }
    }
}