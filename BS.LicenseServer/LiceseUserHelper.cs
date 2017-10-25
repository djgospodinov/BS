using BS.Common.Models;
using BS.LicenseServer.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.LicenseServer
{
    public class LiceseUserHelper
    {
        public static LicenserInfoModel FromDb(LicenseOwner owner)
        {
            if (owner == null)
                return null;

            return new LicenserInfoModel()
                        {
                            Id = owner.Id,
                            Name = owner.Name,
                            IsCompany = owner.IsCompany,
                            Email = owner.Email,
                            Phone = owner.Phone,
                            ConactPerson = owner.ContactPerson,
                            CompanyId = owner.CompanyId,
                        };
        }
    }
}
