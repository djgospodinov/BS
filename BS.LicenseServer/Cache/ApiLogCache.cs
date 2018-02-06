using BS.Common.Models;
using BS.LicenseServer.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.LicenseServer.Cache
{
    public class ApiLogCache : BaseCache
    {
        private List<ApiLogEntry> _requestLogs;
        private readonly object _lock = new object();

        public ApiLogCache()
            : base(new TimeSpan(0, 0, 15))
        {
        }

        protected override void Initiliaze()
        {
            lock (_lock)
            {
                using (var db = new LicenseDbEntities())
                {
                    _requestLogs = db.ApiLogs.Select(x => new ApiLogEntry()
                    {
                        Id = x.Id,
                        RequestContentBody = x.RequestBody,
                        RequestIpAddress = x.RequestIpAddress,
                        RequestMethod = x.RequestMethod,
                        RequestTimestamp = x.RequestTimestamp,
                        RequestUri = x.RequestUri,
                        ResponseContentBody = x.ResponseContentBody,
                        ResponseStatusCode = x.ResponseStatusCode,
                        ResponseTimestamp = x.ResponseTimestamp,
                        AbsoluteUri = x.AbsoluteUri
                    }).ToList();
                }
            }
        }

        public List<ApiLogEntry> GetLogs()
        {
            return _requestLogs.ToList();
        }

        public ApiLogEntry GetLogById(int id)
        {
            return _requestLogs.ToList().FirstOrDefault(x => x.Id == id);
        }
    }
}
