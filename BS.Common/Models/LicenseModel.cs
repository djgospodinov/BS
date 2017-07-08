using System;
using BS.Common;
using BS.Common.Models;
using System.Collections.Generic;
using System.Linq;

namespace BS.Api.Models
{
    public class LicenseModel
    {
        public Guid Id { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsDemo { get; set; }
        public LicenserInfoModel User { get; set; }
        public List<LicenseModules> Modules { get; set; } 

        public override string ToString()
        {
            var modules = Modules != null ? Modules.Select(x => x.Description()) : new string[0];
            return string.Format(
@"Идентификатор: {0},
Валиден до: {1},
Демо: {2},
Потребител: {3},
Mодули: {4}", Id, ValidTo, IsDemo.ToBgString(), User, string.Join(",", modules));
        }
    }
}