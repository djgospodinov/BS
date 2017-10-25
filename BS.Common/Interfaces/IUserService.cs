using BS.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Common.Interfaces
{
    public interface IUserService
    {
        LicenserInfoModel Get(int id);

        LicenserInfoModel Get(string companyId);

        List<LicenserInfoModel> GetAll(bool? isDemo = null);
    }
}
