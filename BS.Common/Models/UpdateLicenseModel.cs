using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BS.Common.Models
{
    [DataContract]
    public class UpdateLicenseModel
    {
        [DataMember]
        public DateTime? ValidTo { get; set; }

        [DataMember]
        public DateTime? SubscribedTo { get; set; }

        [DataMember]
        public bool? IsDemo { get; set; }

        [DataMember]
        public int? UserId { get; set; }

        [DataMember]
        public List<LicenseModulesEnum> Modules { get; set; }

        [DataMember]
        public LicenseTypeEnum? Type { get; set; }

        [DataMember]
        public bool? Enabled { get; set; }

        [DataMember]
        public string ActivationId { get; set; }

        [DataMember]
        public bool? IsActivated { get; set; }
    }
}
