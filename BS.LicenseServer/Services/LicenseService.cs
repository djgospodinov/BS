using BS.Api.Common;
using BS.Common.Models;
using BS.LicenseServer.Db;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BS.LicenseServer.Services
{
    public class LicenseService : ILicenseService
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        public Api.Models.LicenseModel Get(string id)
        {
            using (var db = new LicenseDbEntities())
            {
                var guid = Guid.Parse(id);
                var result = db.Licenses.FirstOrDefault(x => x.Id == guid);
                if (result != null)
                {
                    return new Api.Models.LicenseModel()
                    {
                        Id = result.Id,
                        IsDemo = result.IsDemo,
                        ValidTo = result.ValidTo,
                        Type = (LicenseType)result.Type,
                        User = new Common.Models.RealLicenserInfoModel()
                        {
                            Name = result.LicenseOwner.Name,
                            IsCompany = result.LicenseOwner.IsCompany,
                            Email = result.LicenseOwner.Email,
                            Phone = result.LicenseOwner.Phone,
                            ConactPerson = result.LicenseOwner.ContactPerson,
                            CompanyId = result.LicenseOwner.CompanyId,
                        },
                        Modules = result.LicenseModules.Select(x => (LicenseModulesEnum)x.ModuleId).ToList()
                    };
                }
            }

            return null;
        }

        public List<Api.Models.LicenseModel> GetByFilter(LicenseFilterModel filter)
        {
            using (var db = new LicenseDbEntities())
            {
                var result = db.Licenses.Where(x => x.LicenseOwner.IsCompany == true
                    && x.LicenseOwner.CompanyId == filter.CompanyId)
                    .ToList();

                return result
                    .Select(x => new Api.Models.LicenseModel()
                    {
                        Id = x.Id,
                        IsDemo = x.IsDemo,
                        ValidTo = x.ValidTo,
                        User = new Common.Models.RealLicenserInfoModel()
                        {
                            Name = x.LicenseOwner.Name,
                            IsCompany = x.LicenseOwner.IsCompany,
                            Email = x.LicenseOwner.Email,
                            Phone = x.LicenseOwner.Phone,
                            ConactPerson = x.LicenseOwner.ContactPerson,
                            CompanyId = x.LicenseOwner.CompanyId
                        },
                        Modules = x.LicenseModules.Select(m => (LicenseModulesEnum)m.ModuleId).ToList()
                    })
                    .ToList();
            }
        }

        public string Create(Api.Models.LicenseModel model)
        {
            using (var db = new LicenseDbEntities())
            {
                var owner = db.LicenseOwners.FirstOrDefault(x => x.CompanyId != null && x.CompanyId == model.User.CompanyId)
                    ?? new LicenseOwner()
                    {
                        Name = model.User.Name,
                        IsCompany = model.User.IsCompany,
                        Email = model.User.Email,
                        Phone = model.User.Phone,
                        ContactPerson = model.User.ConactPerson,
                        CompanyId = model.User.CompanyId
                    };

                var extraInfo = owner.LicenseOwnerExtraInfoes.FirstOrDefault();
                if (extraInfo == null)
                {
                    extraInfo = new LicenseOwnerExtraInfo();

                    if (model.User is RealLicenserInfoModel)
                    {
                        extraInfo.LicenseOwnerId = owner.Id;

                        var userInfo = (RealLicenserInfoModel)model.User;

                        extraInfo.PostCode = userInfo.PostCode;
                        extraInfo.PostAddress = userInfo.PostAddress;
                        extraInfo.RegistrationAddress = userInfo.RegistrationAddress;
                        extraInfo.MOL = userInfo.MOL;
                        extraInfo.ContactPerson = userInfo.ConactPerson;
                        extraInfo.AccountingPerson = userInfo.AccountingPerson;
                        extraInfo.DDSRegistration = userInfo.DDSRegistration;

                        owner.LicenseOwnerExtraInfoes.Add(extraInfo);
                    }
                }

                var result = new License()
                {
                    Id = Guid.NewGuid(),
                    IsDemo = model.IsDemo,
                    ValidTo = model.ValidTo,
                    SubscribedTo = model.SubscribedTo,
                    Type = (byte)model.Type,
                    LicenseOwner = owner,
                    LicenseModules = model.Modules.Select(x => new LicenseModule() { ModuleId = (short)x }).ToList()
                };

                var created = db.Licenses.Add(result);
                db.SaveChanges();

                return created.Id.ToString();
            }
        }

        public bool Update(string id, Api.Models.LicenseModel model)
        {
            using (var db = new LicenseDbEntities())
            {
                var result = db.Licenses.FirstOrDefault(x => x.Id == new Guid(id));
                if (result != null) 
                {
                    result.ValidTo = model.ValidTo;
                    result.SubscribedTo = model.SubscribedTo;
                    result.IsDemo = model.IsDemo;

                    result.LicenseOwner.Name = model.User.Name;
                    result.LicenseOwner.IsCompany = model.User.IsCompany;
                    result.LicenseOwner.Email = model.User.Email;
                    result.LicenseOwner.Phone = model.User.Phone;
                    result.LicenseOwner.ContactPerson = model.User.ConactPerson;
                    result.LicenseOwner.CompanyId = model.User.CompanyId;

                    var extraInfo = result.LicenseOwner.LicenseOwnerExtraInfoes.FirstOrDefault();
                    if (extraInfo == null) 
                       extraInfo = new LicenseOwnerExtraInfo();

                    if (model.User is RealLicenserInfoModel)
                    {
                        extraInfo.LicenseOwnerId = result.LicenseOwner.Id;

                        var userInfo = (RealLicenserInfoModel)model.User;

                        extraInfo.PostCode = userInfo.PostCode;
                        extraInfo.PostAddress = userInfo.PostAddress;
                        extraInfo.RegistrationAddress = userInfo.RegistrationAddress;
                        extraInfo.MOL = userInfo.MOL;
                        extraInfo.ContactPerson = userInfo.ConactPerson;
                        extraInfo.AccountingPerson = userInfo.AccountingPerson;
                        extraInfo.DDSRegistration = userInfo.DDSRegistration;

                        result.LicenseOwner.LicenseOwnerExtraInfoes.Add(extraInfo);
                    }

                    db.SaveChanges();

                    return true;
                }
            }

            return false;
        }

        public bool Delete(string id)
        {
            using (var db = new LicenseDbEntities())
            {
                var result = db.Licenses.FirstOrDefault(x => x.Id == new Guid(id));
                if (result != null) 
                {
                    result.Enabled = false;
                    db.SaveChanges();

                    return true;
                }
            }

            return false;
        }


        public string[] CreateMany(List<Api.Models.LicenseModel> model)
        {
            var result = new List<string>();
            using (var scope = new TransactionScope()) 
            {
                foreach (var m in model) 
                {
                    var id = Create(m);
                    result.Add(id);
                }

                scope.Complete();
            }

            return result.ToArray();
        }
    }
}
