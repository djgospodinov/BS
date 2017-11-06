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
        [Display(Name = "Идентификатор")]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$")]
        [Display(Name = "Ip Адрес")]
        public string IpAddress { get; set; }

        [Required]
        [Display(Name = "Забранен")]
        public bool IsDenied { get; set; }
    }
}