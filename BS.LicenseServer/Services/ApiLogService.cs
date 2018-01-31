using BS.Common.Models;
using BS.LicenseServer.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.LicenseServer.Services
{
    public class ApiLogService
    {
        public static void Log(ApiLogEntry logEntry)
        {
            using (var db = new LicenseDbEntities())
            {
                var result = new ApiLog()
                {
                    RequestBody = logEntry.RequestContentBody,
                    RequestIpAddress = logEntry.RequestIpAddress,
                    RequestMethod = logEntry.RequestMethod,
                    RequestTimestamp = logEntry.RequestTimestamp.Value,
                    RequestUri = logEntry.RequestUri,
                    ResponseContentBody = logEntry.ResponseContentBody,
                    ResponseStatusCode = logEntry.ResponseStatusCode,
                    ResponseTimestamp = logEntry.ResponseTimestamp
                };

                db.ApiLogs.Add(result);
                db.SaveChanges();
            }
        }

        public static List<ApiLogEntry> GetLogs()
        {
            using (var db = new LicenseDbEntities())
            {
                return db.ApiLogs.Select(x => new ApiLogEntry()
                {
                    RequestContentBody = x.RequestBody,
                    RequestIpAddress = x.RequestIpAddress,
                    RequestMethod = x.RequestMethod,
                    RequestTimestamp = x.RequestTimestamp,
                    RequestUri = x.RequestUri,
                    ResponseContentBody = x.ResponseContentBody,
                    ResponseStatusCode = x.ResponseStatusCode,
                    ResponseTimestamp = x.ResponseTimestamp
                }).ToList();
            }
        }
    }
}
