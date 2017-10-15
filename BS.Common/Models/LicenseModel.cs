using System;
using BS.Common;
using BS.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace BS.Common
{
    public class LicenseModel
    {
        public Guid Id { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        public DateTime? SubscribedTo { get; set; }

        [Required]
        public bool IsDemo { get; set; }

        [Required]
        public LicenserInfoModelBase User { get; set; }

        [Required]
        public List<LicenseModulesEnum> Modules { get; set; }

        [Required]
        public LicenseType  Type { get; set; }
        
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