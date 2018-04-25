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
        [Display(Name ="ClientId")]
        public string ClientId { get; set; }
        [Display(Name = "Име/Потребител")]
        public string CompanyName { get; set; }
        [Display(Name = "Вид")]
        public string LicenseType { get; set; }
        [Display(Name = "Брой компютри")]
        public int WorkStationsCount { get; set; }
        [Display(Name = "ClientId")]
        public List<LicenseActivationsInfoModel> LicenseActivationsInfo { get;set;}
    }

    public class LicenseActivationsInfoModel
    {
        public string ComputerName { get; set; }
        public List<LicenseModulesEnum> Modules { get; set; }
        public Guid LicenseId { get; set; }
        public DateTime? ValidTo { get; set; }
        public DateTime? UpdatedTo { get; set; }
        public bool IsActivated { get; set; }
        public bool Enabled { get; set; }
    }
}