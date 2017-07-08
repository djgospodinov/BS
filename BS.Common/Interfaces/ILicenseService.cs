using BS.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Api.Common
{
    public interface ILicenseService
    {
        LicenseModel Get(string id);

        string Create(LicenseModel model);

        bool Update(string id, LicenseModel model);

        bool Delete(string id);
    }
}