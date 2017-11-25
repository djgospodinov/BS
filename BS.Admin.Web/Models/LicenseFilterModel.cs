using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Admin.Web.Models
{
    public class LicenseGridModel 
    {
        public int? UserId { get; set; }
    }

    public class UserLicenseGridModel
    {
        public bool Demo { get; set; }

        public string Url 
        {
            get 
            {
                return string.Format("{0}?demo={1}", "UserLicense/Data", Demo);
            } 
        }

        public string DemoId 
        { 
            get 
            {
                return Demo ? "demo" : "real";
            } 
        }
    }

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
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public bool? Demo { get; set; }
        public bool? Activated { get; set; }
        public bool? Enabled { get; set; }
        public int Type { get; set; }
        
    }

    public class UserLicenseFilterGridModel : FilterGridModelBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool? Company { get; set; }
        public string CompanyId { get; set; }
        public bool Demo { get; set; }
    }
}