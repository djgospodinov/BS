using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Common.Models
{
    public enum LicenseTypeEnum
    {
        [Description("За компютър")]
        PerComputer = 1,

        [Description("За потребител")]
        PerUser = 2,

        [Description("За сървър")]
        PerServer = 3
    }
}
