using BS.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Api.Services
{
    public interface ILicenseService
    {
        LicenseModel Get(string id);

        string Create(LicenseModel model);

        bool Update(LicenseModel model);

        bool Delete(string id);
    }
}