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
        /// <summary>
        /// the id of the license
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// datetime showing when the license is valid to
        /// </summary>
        [DataMember]
        [Required]
        public DateTime ValidTo { get; set; }

        /// <summary>
        /// shows when the license has subscription for support
        /// </summary>
        [DataMember]
        public DateTime? SubscribedTo { get; set; }

        /// <summary>
        /// if the license is demo or not
        /// </summary>
        [DataMember]
        [Required]
        public bool IsDemo { get; set; }

        /// <summary>
        /// information about the user that bought the license
        /// </summary>
        [DataMember]
        [Required]
        public LicenserInfoModel User { get; set; }

        [Required]
        public List<LicenseModulesEnum> LicenseModules { get; set; }

        /// <summary>
        /// what license modules have been bought
        /// </summary>
        [DataMember]
        public List<LicenseModuleModel> Modules { get; set; }

        /// <summary>
        /// the type of the license.possible values are PerComputer, PerUser, PerServer
        /// </summary>
        [DataMember]
        [Required]
        public LicenseTypeEnum  Type { get; set; }

        /// <summary>
        /// is the license enabled, shows if the license is paid
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        public string ActivationId { get; set; }

        /// <summary>
        /// how many workstations can be used for this license, this is valid only for PerComputer type.
        /// </summary>
        [DataMember]
        public int? WorkstationsCount { get; set; }

        /// <summary>
        /// is the license activated, shows if the license is send activation key
        /// </summary>
        [DataMember]
        public bool IsActivated { get; set; }

        /// <summary>
        /// when has been created
        /// </summary>
        [DataMember]
        public DateTime Created { get; set; }

        public override string ToString()
        {
            var modules = LicenseModules != null ? LicenseModules.Select(x => x.Description()) : new string[0];
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