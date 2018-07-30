using BS.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BS.Admin.Web.Models
{
    public class LicenseAndUsersInfoModel
    {
        public string Id { get; set; }
        [Display(Name = "Демо")]
        public bool IsDemo { get; set; }
        [Display(Name ="ClientId")]
        public string ClientId { get; set; }
        [Display(Name = "Име/Потребител")]
        public string CompanyName { get; set; }
        [Display(Name = "Вид")]
        public string LicenseType { get; set; }
        [Display(Name = "Брой компютри")]
        public int WorkStationsCount { get; set; }
    }

    public class LicenseActivationsInfoModel
    {
        public string ComputerName { get; set; }
        public Guid LicenseId { get; set; }
        public string ValidTo { get; set; }
        public string UpdatedTo { get; set; }
        public bool IsActivated { get; set; }
        public bool Enabled { get; set; }
        public bool Accounting { get; set; }
        public bool Payroll { get; set; }
        public bool Schedule { get; set; }
        public bool Store { get; set; }
        public bool Production { get; set; }
    }
}