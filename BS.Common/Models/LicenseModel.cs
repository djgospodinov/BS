using System;
using BS.Common;
using BS.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BS.Common
{
    [DataContract]
    public class LicenseModel
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        [Required]
        public DateTime ValidTo { get; set; }

        [DataMember]
        public DateTime? SubscribedTo { get; set; }

        [DataMember]
        [Required]
        public bool IsDemo { get; set; }

        [DataMember]
        [Required]
        public LicenserInfoModel User { get; set; }

        [DataMember]
        [Required]
        public List<LicenseModulesEnum> Modules { get; set; }

        [DataMember]
        [Required]
        public LicenseTypeEnum  Type { get; set; }

        [DataMember]
        public bool Enabled { get; set; }

        public string ActivationId { get; set; }

        [DataMember]
        public int? ComputerCount { get; set; }

        [DataMember]
        public bool IsActivated { get; set; }

        [DataMember]
        public DateTime Created { get; set; }

        public override string ToString()
        {
            var modules = Modules != null ? Modules.Select(x => x.Description()) : new string[0];
            return string.Format(
@"Идентификатор: {0},
Валиден до: {1},
Демо: {2},
Потребител: {3},
Mодули: {4},
Вид: {5}", Id, ValidTo, IsDemo.ToBgString(), User, string.Join(",", modules), Type.Description());
        }
    }
}