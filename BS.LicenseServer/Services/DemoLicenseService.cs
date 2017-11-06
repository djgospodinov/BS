using BS.Common;
using BS.Common.Exceptions;
using BS.Common.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Api.Services
{
    public class DemoLicenseService : ILicenseService
    {
        protected static readonly Dictionary<string, LicenseModel> _licenses = new Dictionary<string, LicenseModel>();

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        static DemoLicenseService() 
        {
            var license = new LicenseModel() 
            {
                Id = Guid.Parse("9e35c57e-eecc-4123-a5dc-f914ccb89545"),
                ValidTo = DateTime.Now.AddDays(30)
            };

            _licenses.Add(license.Id.ToString(), license);
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

        public string Create(LicenseModel model)
        {
            model.Id = Guid.NewGuid();

            if (model.IsDemo) 
            {
                model.ValidTo = DateTime.Now.AddMonths(3);
            }

            string id = model.Id.ToString().Replace("-", "");
            _licenses.Add(id, model);

            return id;
        }

        public bool Update(string id, UpdateLicenseModel model)
        {
            try 
            {
                LicenseModel existingModel;
                if (_licenses.TryGetValue(id, out existingModel)) 
                {
                    return true;
                }
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, string.Format("License with Id: {0}", id), ex);

                return false;
            }

            return false;
        }

        public bool Delete(string id)
        {
            return _licenses.Remove(id);
        }


        public List<LicenseModel> GetByFilter(BS.Common.Models.LicenseFilterModel filter)
        {
            throw new NotImplementedException();
        }


        public string[] CreateMany(List<LicenseModel> model)
        {
            throw new NotImplementedException();
        }


        public List<LicenseModel> GetAll()
        {
            return null;
        }


        public void Activate(LicenseModel license, string activationId)
        {
            
        }
    }
}