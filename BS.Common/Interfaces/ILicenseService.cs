using BS.Common;
using BS.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Common
{
    public interface ILicenseService
    {
        LicenseModel Get(string id);

        List<LicenseActivationModel> LicenseActivations(string id);

        List<LicenseModel> GetByFilter(LicenseFilterModel filter);

        List<LicenseModel> GetAll();

        string Create(LicenseModel model);

        string[] CreateMany(List<LicenseModel> model);

        bool Update(string id, UpdateLicenseModel model);

        bool Delete(string id);

        bool CheckOrActivate(LicenseModel license, string activationKey, string computerName);

        bool Activate(Guid id, LicenseActivateModel model);
    }
}