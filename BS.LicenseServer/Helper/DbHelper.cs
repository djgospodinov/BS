using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS.Common.Models;
using BS.LicenseServer.Db;

namespace BS.LicenseServer.Helper
{
    public class DbHelper
    {
        public static LicenseOwner CreateDbModel(LicenserInfoModel model, LicenseOwner dbModel = null)
        {
            var result = dbModel ?? new LicenseOwner();
            result.Name = model.Name;
            result.Phone = model.Phone;
            result.Email = model.Email;
            result.ContactPerson = model.ContactPerson;
            result.CompanyId = model.CompanyId;
            result.EGN = model.EGN;
            result.IsCompany = model.IsCompany;

            if (!model.IsDemo)
            {
                var extraInfo = result.LicenseOwnerExtraInfoes1.FirstOrDefault();
                if (extraInfo == null)
                {
                    extraInfo = new LicenseOwnerExtraInfo1();
                    result.LicenseOwnerExtraInfoes1.Add(extraInfo);
                }

                extraInfo.AccountingPerson = model.AccountingPerson;
                extraInfo.ContactPerson = model.ContactPerson;
                extraInfo.VatRegistration = model.VATRegistration;
                extraInfo.MOL = model.MOL;
                extraInfo.PostAddress = model.PostAddress;
                extraInfo.PostCode = model.PostCode;
                extraInfo.RegistrationAddress = model.RegistrationAddress;
            }
            return result;
        }

        public static LicenserInfoModel FromDbModel(LicenseOwner model)
        {
            if (model == null)
                return null;

            var result = new LicenserInfoModel();
            result.Id = model.Id;
            result.Name = model.Name;
            result.Phone = model.Phone;
            result.Email = model.Email;
            result.ContactPerson = model.ContactPerson;
            result.CompanyId = model.CompanyId;
            result.IsCompany = model.IsCompany;
            result.EGN = model.EGN;
            result.IsDemo = model.Licenses.All(x => x.IsDemo);

            var extraInfo = model.LicenseOwnerExtraInfoes1.FirstOrDefault();
            if (extraInfo != null)
            {
                result.AccountingPerson = extraInfo.AccountingPerson;
                result.ContactPerson = extraInfo.ContactPerson;
                result.VATRegistration = extraInfo.VatRegistration ?? false;
                result.MOL = extraInfo.MOL;
                result.PostAddress = extraInfo.PostAddress;
                result.PostCode = extraInfo.PostCode ?? 0;
                result.RegistrationAddress = extraInfo.RegistrationAddress;
            }

            return result;
        }
    }
}
