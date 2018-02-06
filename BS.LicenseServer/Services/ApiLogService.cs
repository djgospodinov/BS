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
        private static ApiLogCache _apiCache = new ApiLogCache();

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

        public List<ApiLogEntry> GetLogs()
        {
            return _apiCache.GetLogs();
        }

        public ApiLogEntry GetLogEntry(int id)
        {
            return _apiCache.GetLogById(id);
        }
        #endregion

        #region License Log
        public List<LicenseLogModel> GetLicenseLogs()
        {
            return _licenseCache.GetLogs();
        }

        public LicenseLogModel GetLicenseLog(int id)
        {
            return _licenseCache.GetLogs()
                .FirstOrDefault(x => x.Id == id);
        }
        #endregion

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
