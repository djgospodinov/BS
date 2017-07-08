using BS.Api.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.LicenseServer.Services
{
    public class LicenseService : ILicenseService
    {
        //private readonly _db = new DB();
        public Api.Models.LicenseModel Get(string id)
        {
            throw new NotImplementedException();
        }

        public string Create(Api.Models.LicenseModel model)
        {
            throw new NotImplementedException();
        }

        public bool Update(string id, Api.Models.LicenseModel model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
