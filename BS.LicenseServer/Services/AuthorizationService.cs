using BS.Common.Interfaces;
using BS.LicenseServer.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.LicenseServer.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public bool Authorize(string key)
        {
            using (var db = new LicenseDbEntities())
            {
                return db.ApiKeys.Any(x => x.ApiKey1 == key && x.Enabled);
            }
        }
    }
}
