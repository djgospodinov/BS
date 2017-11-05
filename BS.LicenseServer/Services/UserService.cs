using BS.Common.Interfaces;
using BS.Common.Models;
using BS.LicenseServer.Db;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.LicenseServer.Services
{
    public class UserService : IUserService
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        public Common.Models.LicenserInfoModel Get(int id)
        {
            using (var db = new LicenseDbEntities())
            {
                return FromDbModel(db.LicenseOwners.FirstOrDefault(x => x.Id == id));
            }
        }

        public Common.Models.LicenserInfoModel Get(string companyId)
        {
            throw new NotImplementedException();
        }

        public List<Common.Models.LicenserInfoModel> GetAll(bool? isDemo = null)
        {
            using (var db = new LicenseDbEntities())
            {
                return db.LicenseOwners
                    .Where(x => !isDemo.HasValue
                        || (isDemo.Value && string.IsNullOrEmpty(x.CompanyId)))
                    .Select(x => new LicenserInfoModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsCompany = x.IsCompany,
                        Email = x.Email,
                        Phone = x.Phone,
                        ContactPerson = x.ContactPerson,
                        CompanyId = x.CompanyId,
                        IsDemo = string.IsNullOrEmpty(x.CompanyId)
                    })
                    .ToList();
            }
        }

        public bool Create(LicenserInfoModel model)
        {
            try
            {
                using (var db = new LicenseDbEntities())
                {
                    var result = CreateDbModel(model);

                    db.LicenseOwners.Add(result);
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                var error = ((System.Data.Entity.Validation.DbEntityValidationException)ex).EntityValidationErrors;
                _logger.Log(LogLevel.Error, ex);

                throw;
            }
        }

        #region Helper Methods
        private static LicenseOwner CreateDbModel(LicenserInfoModel model, LicenseOwner dbModel = null)
        {
            var result = dbModel ?? new LicenseOwner();
            result.Name = model.Name;
            result.Phone = model.Phone;
            result.Email = model.Email;
            result.ContactPerson = model.ContactPerson;
            result.CompanyId = model.CompanyId;
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
                extraInfo.DDSRegistration = model.DDSRegistration;
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
            result.IsDemo = !model.Licenses.Any(x => !x.IsDemo);

            var extraInfo = model.LicenseOwnerExtraInfoes1.FirstOrDefault();
            if (extraInfo != null)
            {
                result.AccountingPerson = extraInfo.AccountingPerson;
                result.ContactPerson = extraInfo.ContactPerson;
                result.DDSRegistration = extraInfo.DDSRegistration.HasValue ? extraInfo.DDSRegistration.Value : false;
                result.MOL = extraInfo.MOL;
                result.PostAddress = extraInfo.PostAddress;
                result.PostCode = extraInfo.PostCode.HasValue ? extraInfo.PostCode.Value : 0;
                result.RegistrationAddress = extraInfo.RegistrationAddress;
            }

            return result;
        }
        #endregion

        public bool Update(int id, LicenserInfoModel model)
        {
            try
            {
                using (var db = new LicenseDbEntities())
                {
                    var dbModel = db.LicenseOwners.FirstOrDefault(x => x.Id == id);
                    if (dbModel != null) 
                    {
                        var result = CreateDbModel(model, dbModel);

                        db.SaveChanges();

                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                var error = ((System.Data.Entity.Validation.DbEntityValidationException)ex).EntityValidationErrors;
                _logger.Log(LogLevel.Error, ex);

                throw;
            }
        }
    }
}
