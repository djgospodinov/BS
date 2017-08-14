using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BS.Api.Models
{
    public class IpAddressElement : ConfigurationElement
    {
        [ConfigurationProperty("address", IsKey = true, IsRequired = true)]
        public string Address
        {
            get { return (string)this["address"]; }
            set { this["address"] = value; }
        }

        [ConfigurationProperty("denied", IsRequired = false)]
        public bool Denied
        {
            get { return (bool)this["denied"]; }
            set { this["denied"] = value; }
        }
    }

    [ConfigurationCollection(typeof(IpAddressElement))]
    public class IpAddressElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new IpAddressElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IpAddressElement)element).Address;
        }
    }

    public class IpFilteringSection : ConfigurationSection
    {
        [ConfigurationProperty("ipAddresses", IsDefaultCollection = true)]
        public IpAddressElementCollection IpAddresses
        {
            get { return (IpAddressElementCollection)this["ipAddresses"]; }
            set { this["ipAddresses"] = value; }
        }
    }
}