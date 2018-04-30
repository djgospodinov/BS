using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Common.Models.Requests
{
    public class AddServerRequest
    {
        [Required]
        /// <summary>
        /// уникален наш код на клиента, съществува в таблица LicenseOwners и чрез него може да се вземе LicenseOwners.Id и да се запише в LicenseOwnerServer.LicenseOwnerID
        /// </summary>
        public string RegNom { get; set; }
        [Required]
        /// <summary>
        /// Дата на вземане на данните, записваме го в LicenseOwnerServer.CreateDate
        /// </summary>
        public DateTime RequestDate { get; set; }
        [Required]
        /// <summary>
        /// Компютъра, от който е изпратена заявката. Записваме го в LicenseOwnerServer.SendFromPC
        /// </summary>
        public string ComputerName { get; set; }
        /// <summary>
        /// IP-то, незадължително, ако го подадем , ще го записваме в  LicenseOwnerServer.SendFromPCIPAddress
        /// </summary>
        public string ComputerIP { get; set; }
        /// <summary>
        /// Windows User Name – записваме го в LicenseOwnerServer.SystemUserName
        /// </summary>
        public string SystemUserName { get; set; }
        [Required]
        public List<AddServerInfo> Servers { get; set; }
    }

    public class AddServerInfo
    {
        [Required]
        /// <summary>
        ///  Име на SQL Server Instance – Записваме го в LicenseOwnerServer.ServerName
        /// </summary>
        public string ServerInstance { get; set; }
        [Required]
        /// <summary>
        ///  IP-то на SQL Server Instance, незадължително –Записваме го в LicenseOwnerServer.ServerIPAddress
        /// </summary>
        public string ServerIPAddress { get; set; }
    }

    public class AddServerResponse
    {
        public bool AddedServer { get; set; }
    }
}
