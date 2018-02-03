using BS.Common.Models;
using BS.LicenseServer.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BS.LicenseServer.Cache
{
    public class IpFilterCache : BaseCache
    {
        private bool _useIpFiltering = false;
        private List<IpAddressElement> _ipAddress;
        private static object _lock = new object();

        public static readonly IpFilterCache Instance = new IpFilterCache();

        private IpFilterCache()
            : base(new TimeSpan(0, 5, 0))
        {
        }

        protected override void Initiliaze()
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
                            Address = x.Address.Trim(),
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
