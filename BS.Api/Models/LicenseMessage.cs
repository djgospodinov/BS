using BS.Common;
using BS.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using BS.Common;

namespace BS.Api.Models
{
    public class LicenseMessage
    {
        public LicenseMessage()
        {
        }

        public LicenseMessage(LicenseModel x)
        {
            Id = x.Id;
            ValidTo = x.ValidTo;
            SubscribedTo = x.SubscribedTo;
            IsDemo = x.IsDemo;
            User = x.User;
            Modules = x.Modules.Select(v => v.Code).ToList();
            Type = x.Type.ToString();
            Enabled = x.Enabled;
            WorkstationsCount = x.WorkstationsCount;
            IsActivated = x.IsActivated;
            Created = x.Created;
        }

        /// <summary>
        /// the id of the license
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// datetime showing when the license is valid to
        /// </summary>
        [DataMember]
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
        public bool IsDemo { get; set; }

        /// <summary>
        /// information about the user that bought the license
        /// </summary>
        [DataMember]
        public LicenserInfoModel User { get; set; }

        /// <summary>
        /// what license modules have been bought
        /// the code of the module, possible values are: Accounting, Payroll, Store, Schedule
        /// </summary>
        [DataMember]
        public List<string> Modules { get; set; }

        /// <summary>
        /// the type of the license.possible values are PerComputer, PerUser, PerServer
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// is the license enabled, shows if the license is paid
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

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
    }
}