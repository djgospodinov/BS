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
        protected static readonly Dictionary<string, LicenseModel> _licenses = new Dictionary<string, LicenseModel>();

        static LicenseService() 
        {
            var license = new LicenseModel() 
            {
                Id = Guid.Parse("9e35c57e-eecc-4123-a5dc-f914ccb89545"),
                ValidTo = DateTime.Now.AddDays(30)
            };

            _licenses.Add(license.Id.ToString().Replace("-", ""), license);
            //licenses.Add("a8650fa262b74405b3329b6d989fbd3e", new LicenseModel()
            //{
            //    ValidTo = DateTime.Now.AddDays(45)
            //});
            //licenses.Add("f7fee614689641f0ba9eb64c3976d51e", new LicenseModel()
            //{
            //    ValidTo = DateTime.Now.AddDays(60)
            //});
        }

        public LicenseModel Get(string id)
        {
            LicenseModel result;
            if (_licenses.TryGetValue(id, out result))
            {
                return result;
            }

            throw new LicenseNotFoundException(string.Format("License for ID: {0} was not found.", id));
        }

        public string Create(Models.LicenseModel model)
        {
            model.Id = Guid.NewGuid();

            string id = model.Id.ToString().Replace("-", "");
            _licenses.Add(id, model);

            return id;
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