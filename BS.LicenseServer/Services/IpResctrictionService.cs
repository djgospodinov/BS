using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS.Common.Models;

namespace BS.LicenseServer.Services
{
    public class IpResctrictionService
    {
        private static bool _useIpFiltering = false;//
        private static List<IpAddressElement> _ipAddress; 
        private static object _lock = new object();

        public static List<IpAddressElement> GetAll()
        {
            if (!_useIpFiltering)
                return null;

            if (_ipAddress == null)
            {
                lock (_lock)
                {
                    _ipAddress = new List<IpAddressElement>();
                }
            }

            return _ipAddress;
        }
    }
}
