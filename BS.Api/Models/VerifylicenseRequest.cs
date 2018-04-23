using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Api.Models
{
    /// <summary>
    /// Параметри на заявката за проверка или активация на лиценз
    /// </summary>
    public class VerifyLicenseRequest
    {
        /// <summary>
        /// Стойност която отговаря на генериран идентификатор на компютър или на потребител, това зависи от вида лиценза
        /// </summary>
        public string ActivationKey { get; set; }
        /// <summary>
        /// Името на работната станция, където се лицензира
        /// </summary>
        public string ComputerName { get; set; }
    }

    public class VerifyLicenseRequestEx : VerifyLicenseRequest
    {
        /// <summary>
        /// Сървъра, който използва Мираж
        /// </summary>
        public string ServerName { get; set; }
    }
}