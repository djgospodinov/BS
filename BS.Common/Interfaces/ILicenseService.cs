using BS.Api.Models;
using BS.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Api.Common
{
    public interface ILicenseService
    {
        LicenseModel Get(string id);

        List<LicenseModel> GetByFilter(LicenseFilterModel filter);

        string Create(LicenseModel model);

        string[] CreateMany(List<LicenseModel> model);

        bool Update(string id, LicenseModel model);

        bool Delete(string id);
    }
}