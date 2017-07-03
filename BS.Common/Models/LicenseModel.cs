using System;
using BS.Common;
using BS.Common.Models;
using System.Collections.Generic;

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
            return string.Format(
@"Идентификатор: {0},
Валиден до: {1},
Демо: {2},
Потребител: {3}", Id, ValidTo, IsDemo.ToBgString(), User);
        }
    }
}