using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BS.Common.Models
{
    //TODO: Remove this enum, instead load it from the db
    public enum LicenseModulesEnum
    {
        [Description("Счетоводство")]
        Accounting = 1,

        [Description("ТРЗ и личен състав")]
        Payroll = 2,

        [Description("Складов софтуер")]
        Store = 3,

        [Description("Графици на работа")]
        Schedule = 4
    }

    [DataContract]
    public class LicenseModuleModel
    {
        public int Id { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public DateTime ValidTo { get; set; }
    }
}
