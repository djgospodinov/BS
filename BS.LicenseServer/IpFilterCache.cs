using BS.Common.Models;
using BS.LicenseServer.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BS.LicenseServer
{
    public class IpFilterCache
    {
        private bool _useIpFiltering = false;
        private List<IpAddressElement> _ipAddress;
        private static object _lock = new object();

        public static readonly IpFilterCache Instance = new IpFilterCache();

        private IpFilterCache()
        {
            Initiliaze();

            var timer = new Timer();
            timer.Interval = new TimeSpan(0, 5, 0).TotalMilliseconds;
            timer.Elapsed += (s, e) =>
            {
                Initiliaze();
            };
        }

        private void Initiliaze()
        {
            using (var db = new LicenseDbEntities())
            {
                var row = db.Settings.FirstOrDefault();
                _useIpFiltering = row != null ? row.UseIPFilter : false;

                if (_useIpFiltering)
                {
                    lock (_lock)
                    {
                        _ipAddress = db.IpFilters.Select(x => new IpAddressElement()
                        {
                            Id = x.Id,
                            Address = x.Address,
                            Denied = x.Denied
                        }).ToList();
                    }
                }
            }
        }

        public List<IpAddressElement> GetAll()
        {
            if (!_useIpFiltering)
                return null;

            return _ipAddress;
        }
    }
}
