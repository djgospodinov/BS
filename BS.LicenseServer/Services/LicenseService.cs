using BS.Api.Models;
using BS.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Api.Services
{
    public class LicenseService : ILicenseService
    {
        protected Dictionary<string, LicenseModel> licenses = new Dictionary<string, LicenseModel>();

        public LicenseService() 
        {
            licenses.Add("b0d8dc9d86384b519beb97ae09e39053", new LicenseModel() 
            {
                ValidTo = DateTime.Now.AddDays(30)
            });
            licenses.Add("a8650fa262b74405b3329b6d989fbd3e", new LicenseModel()
            {
                ValidTo = DateTime.Now.AddDays(45)
            });
            licenses.Add("f7fee614689641f0ba9eb64c3976d51e", new LicenseModel()
            {
                ValidTo = DateTime.Now.AddDays(60)
            });
        }

        public LicenseModel Get(string id)
        {
            LicenseModel result;
            if (licenses.TryGetValue(id, out result))
            {
                return result;
            }

            throw new LicenseNotFoundException(string.Format("License for ID: {0} was not found.", id));
        }

        public string Create(Models.LicenseModel model)
        {
            throw new NotImplementedException();
        }

        public bool Update(Models.LicenseModel model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}