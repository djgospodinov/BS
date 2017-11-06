using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BS.Common.Models
{
    public class IpAddressElement
    {
        public string Address { get; set; }

        /// <summary>
        /// this will be used for blacklist/whitelist
        /// </summary>
        public bool Denied { get; set; }
    }
}