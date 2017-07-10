using BS.Api.Common;
using BS.Common.Models;
using BS.LicenseServer.Db;
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
            using (var db = new LicenseDbEntities())
            {
                var result = db.Licenses.FirstOrDefault(x => x.Id == Guid.Parse(id));
                if (result != null) 
                {
                    return new Api.Models.LicenseModel()
                    {
                        Id = result.Id,
                        IsDemo = result.IsDemo,
                        ValidTo = result.ValidTo,
                        User = new Common.Models.LicenserInfoModel() 
                        {
                            Name = result.LicenseOwner.Name,
                            IsCompany = result.LicenseOwner.IsCompany,
                            Email = result.LicenseOwner.Email,
                            Phone = result.LicenseOwner.Phone,
                            ConactPerson = result.LicenseOwner.ContactPerson
                        },
                        Modules = result.LicenseModules.Select(x => (LicenseModulesEnum)x.Id).ToList()
                    };
                }
            }

            return null;
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
