using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Admin.Web.Models
{
    public class FilterGridModelBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SortField { get; set; }
        //"asc"|"desc"
        public string SortOrder { get; set; }
    }

    public class LicenseFilterGridModel : FilterGridModelBase
    {
        public string Id { get; set; }
        
        public int? Тype  { get; set; }
        
    }
}