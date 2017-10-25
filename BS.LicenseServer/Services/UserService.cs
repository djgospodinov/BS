using BS.Common.Interfaces;
using BS.LicenseServer.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.LicenseServer.Services
{
    public class UserService : IUserService
    {
        public Common.Models.LicenserInfoModel Get(int id)
        {
            using (var db = new LicenseDbEntities()) 
            {
                return LiceseUserHelper.FromDb(db.LicenseOwners.FirstOrDefault(x => x.Id == id));
            }
        }

        public Common.Models.LicenserInfoModel Get(string companyId)
        {
            throw new NotImplementedException();
        }

        public List<Common.Models.LicenserInfoModel> GetAll(bool? isDemo = null)
        {
            throw new NotImplementedException();
        }
    }
}
