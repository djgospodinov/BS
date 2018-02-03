using BS.Common.Models;
using BS.LicenseServer.Cache;
using BS.LicenseServer.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.LicenseServer.Services
{
    public class ApiLogService : IDisposable
    {
        private readonly LicenseDbEntities _db = new LicenseDbEntities();

        private static LicenseLogCache _licenseCache = new LicenseLogCache();

        #region Api Log
        public void Log(ApiLogEntry logEntry)
        {
            var result = new ApiLog()
            {
                AbsoluteUri = logEntry.AbsoluteUri,
                Host = logEntry.Host,
                RequestBody = logEntry.RequestContentBody,
                RequestIpAddress = logEntry.RequestIpAddress,
                RequestMethod = logEntry.RequestMethod,
                RequestTimestamp = logEntry.RequestTimestamp.Value,
                RequestUri = logEntry.RequestUri,
                ResponseContentBody = logEntry.ResponseContentBody,
                ResponseStatusCode = logEntry.ResponseStatusCode,
                ResponseTimestamp = logEntry.ResponseTimestamp
            };

            _db.ApiLogs.Add(result);
            _db.SaveChanges();
        }

        public IQueryable<ApiLogEntry> GetLogs()
        {
            return _db.ApiLogs.Select(x => new ApiLogEntry()
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
            });
        }

        public ApiLogEntry GetLogEntry(int id)
        {
            var result = _db.ApiLogs.FirstOrDefault(x => x.Id == id);

            return new ApiLogEntry()
            {
                Id = result.Id,
                RequestContentBody = result.RequestBody,
                RequestIpAddress = result.RequestIpAddress,
                RequestMethod = result.RequestMethod,
                RequestTimestamp = result.RequestTimestamp,
                RequestUri = result.RequestUri,
                ResponseContentBody = result.ResponseContentBody,
                ResponseStatusCode = result.ResponseStatusCode,
                ResponseTimestamp = result.ResponseTimestamp,
                AbsoluteUri = result.AbsoluteUri
            };
        }
        #endregion

        //TODO: LicenseLogCache
        public List<LicenseLogModel> GetLicenseLogs()
        {
            return _licenseCache.GetLogs();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
