using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BS.Admin.Web.Models
{
    public class IpRestrictionModel
    {
        [Display(Name = "Използвай филтър")]
        public bool UseIpRestriction { get; set; }

        public List<IpModel> IPs { get; set; }
    }

    public class IpModel
    {
        [Required]
        [Display(Name = "Ip Адрес")]
        public string IpAddress { get; set; }

        [Required]
        [Display(Name = "Забранено")]
        public bool IsDenied { get; set; }
    }
}