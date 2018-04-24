using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Api.Models
{
    /// <summary>
    /// Отговор към заявката за лицензиране
    /// </summary>
    public class ActivateLicenseMessage
    {
        /// <summary>
        /// Идентификатор за който е лицензиран
        /// </summary>
        public Guid LicenseId { get; internal set; }
        public string ActivationKey { get; set; }
        /// <summary>
        /// Името на работната станция, където е лицензиран
        /// </summary>
        public string ComputerName { get; set; }
        /// <summary>
        /// Сървъра, на който ще се използва Мираж
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// Статус на активацията
        /// </summary>
        public bool Activated { get; internal set; }
    }
}