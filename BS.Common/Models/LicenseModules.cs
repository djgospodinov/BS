﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Common.Models
{
    public enum LicenseModulesEnum
    {
        [Description("Счетоводство")]
        Accounting = 1,

        [Description("Производство")]
        Production = 2,

        [Description("Склад")]
        Warehouse = 3,

        [Description("Търговска система")]
        TradingSystem = 4,

        [Description("ТРЗ")]
        Salary = 5,
            
        [Description("Графици")]
        Schedules = 6
    }

    public class LicenseModuleModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
