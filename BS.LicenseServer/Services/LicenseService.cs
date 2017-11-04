using BS.Common;
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

        public LicenseModel Get(string id)
        {
            using (var db = new LicenseDbEntities())
            {
                var guid = Guid.Parse(id);
                var result = db.Licenses.FirstOrDefault(x => x.Id == guid);
                if (result != null)
                {
                    var activator = result.LicenseActivations.Any() 
                        ? result.LicenseActivations.FirstOrDefault()
                        : null;

                    return new LicenseModel()
                    {
                        Id = result.Id,
                        IsDemo = result.IsDemo,
                        ValidTo = result.ValidTo,
                        Type = (LicenseTypeEnum)result.Type,
                        Enabled = result.Enabled ?? false,
                        SubscribedTo = result.SubscribedTo,
                        Created = result.CreatedDate,
                        User = new LicenserInfoModel()
                        {
                            Id = result.LicenseOwner.Id,
                            Name = result.LicenseOwner.Name,
                            IsCompany = result.LicenseOwner.IsCompany,
                            Email = result.LicenseOwner.Email,
                            Phone = result.LicenseOwner.Phone,
                            ConactPerson = result.LicenseOwner.ContactPerson,
                            CompanyId = result.LicenseOwner.CompanyId,
                        },
                        Modules = result.LicenseModules.Select(x => (LicenseModulesEnum)x.ModuleId).ToList(),
                        ActivationId = ((LicenseTypeEnum)result.Type) == LicenseTypeEnum.PerUser 
                            ? activator != null ? activator.UserId : string.Empty
                            : activator != null ? activator.ComputerId : string.Empty,
                        IsActivated = activator != null
                    };
                }
            }

            return null;
        }

        public List<LicenseModel> GetByFilter(LicenseFilterModel filter)
        {
            using (var db = new LicenseDbEntities())
            {
                var result = db.Licenses.Where(x => x.LicenseOwner.IsCompany == true
                    && x.LicenseOwner.CompanyId == filter.CompanyId)
                    .ToList();

                return result
                    .Select(x => new LicenseModel()
                    {
                        Id = x.Id,
                        IsDemo = x.IsDemo,
                        ValidTo = x.ValidTo,
                        Created = x.CreatedDate,
                        User = new Common.Models.LicenserInfoModel()
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

        public string Create(LicenseModel model)
        {
            try
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

                    var extraInfo = owner.LicenseOwnerExtraInfoes1 != null ? owner.LicenseOwnerExtraInfoes1.FirstOrDefault() : null;
                    if (extraInfo == null)
                    {
                        if (model.User.PostCode > 0
                            || !string.IsNullOrEmpty(model.User.PostAddress)
                            || !string.IsNullOrEmpty(model.User.PostAddress)
                            || !string.IsNullOrEmpty(model.User.RegistrationAddress)
                            || !string.IsNullOrEmpty(model.User.MOL)
                            || !string.IsNullOrEmpty(model.User.AccountingPerson)
                            || !string.IsNullOrEmpty(model.User.ContactPerson))
                        {
                            extraInfo = new LicenseOwnerExtraInfo1();
                            extraInfo.LicenseOwnerId = owner.Id;

                            var userInfo = (LicenserInfoModel)model.User;

                            extraInfo.PostCode = userInfo.PostCode;
                            extraInfo.PostAddress = userInfo.PostAddress;
                            extraInfo.RegistrationAddress = userInfo.RegistrationAddress;
                            extraInfo.MOL = userInfo.MOL;
                            extraInfo.ContactPerson = userInfo.ConactPerson;
                            extraInfo.AccountingPerson = userInfo.AccountingPerson;
                            extraInfo.DDSRegistration = userInfo.DDSRegistration;

                            owner.LicenseOwnerExtraInfoes1.Add(extraInfo);
                        }
                    }

                    var result = new License()
                    {
                        Id = Guid.NewGuid(),
                        IsDemo = model.IsDemo,
                        ValidTo = !model.IsDemo ? model.ValidTo : DateTime.Now.AddMonths(1),
                        SubscribedTo = !model.IsDemo ? model.SubscribedTo : DateTime.Now.AddMonths(1),
                        Type = !model.IsDemo ? (byte)model.Type : (byte)LicenseTypeEnum.PerComputer,
                        LicenseOwner = owner,
                        LicenseModules = model.Modules.Select(x => new LicenseModule() { ModuleId = (short)x }).ToList(),
                        Enabled = !model.IsDemo ? false : true,//the real license should be enabled, afterwards e.g. after it is payed
                        CreatedDate = DateTime.Now
                    };

                    var created = db.Licenses.Add(result);
                    db.SaveChanges();

                    return created.Id.ToString();
                }
            }
            catch (Exception ex)
            {
                var error = ((System.Data.Entity.Validation.DbEntityValidationException) ex).EntityValidationErrors;
                _logger.Log(LogLevel.Error, ex);

                throw;
            }
        }

        public bool Update(string id, LicenseModel model)
        {
            using (var db = new LicenseDbEntities())
            {
                var result = db.Licenses.FirstOrDefault(x => x.Id == new Guid(id));
                if (result != null) 
                {
                    result.ValidTo = model.ValidTo;
                    result.SubscribedTo = model.SubscribedTo;
                    result.IsDemo = model.IsDemo;
                    result.Enabled = model.Enabled;
                    result.Type = (byte)model.Type;

                    if (model.User.Id != result.LicenseOwner.Id) 
                    {
                        result.LicenseOwner = db.LicenseOwners.First(x => x.Id == model.User.Id);
                    }

                    var modulesIds = model.Modules.Select(x => (short)x)
                        .ToList();

                    foreach (var moduleId in modulesIds)
                    {
                        if (result.LicenseModules.FirstOrDefault(x => x.ModuleId == moduleId) == null) 
                        {
                            result.LicenseModules.Add(new LicenseModule()
                            {
                                ModuleId = moduleId
                            });
                        }
                    }

                    var modulesForRemoval = new List<LicenseModule>();
                    foreach (var module in result.LicenseModules) 
                    {
                        if (!modulesIds.Contains(module.ModuleId))
                        {
                            modulesForRemoval.Add(module);
                        }
                    }
                    
                    foreach (var module in modulesForRemoval) 
                    {
                        db.LicenseModules.Remove(module);
                    }

                    #region Update User - should be in update user service
                    //result.LicenseOwner = db.LicenseOwners.First(x => x.Id == model.User.Id);
                    //result.LicenseOwner.Name = model.User.Name;
                    //result.LicenseOwner.IsCompany = model.User.IsCompany;
                    //result.LicenseOwner.Email = model.User.Email;
                    //result.LicenseOwner.Phone = model.User.Phone;
                    //result.LicenseOwner.ContactPerson = model.User.ConactPerson;
                    //result.LicenseOwner.CompanyId = model.User.CompanyId;

                    //var extraInfo = result.LicenseOwner.LicenseOwnerExtraInfoes1.FirstOrDefault();
                    //if (extraInfo == null)
                    //    extraInfo = new LicenseOwnerExtraInfo1();

                    //if (model.User is LicenserInfoModel)
                    //{
                    //    extraInfo.LicenseOwnerId = result.LicenseOwner.Id;

                    //    var userInfo = (LicenserInfoModel)model.User;

                    //    extraInfo.PostCode = userInfo.PostCode;
                    //    extraInfo.PostAddress = userInfo.PostAddress;
                    //    extraInfo.RegistrationAddress = userInfo.RegistrationAddress;
                    //    extraInfo.MOL = userInfo.MOL;
                    //    extraInfo.ContactPerson = userInfo.ConactPerson;
                    //    extraInfo.AccountingPerson = userInfo.AccountingPerson;
                    //    extraInfo.DDSRegistration = userInfo.DDSRegistration;

                    //    result.LicenseOwner.LicenseOwnerExtraInfoes1.Add(extraInfo);
                    //}
                    #endregion

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


        public string[] CreateMany(List<LicenseModel> model)
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


        public List<LicenseModel> GetAll()
        {
            using (var db = new LicenseDbEntities())
            {
                var result = db.Licenses
                    .ToList();

                return result
                    .Select(x => new LicenseModel()
                    {
                        Id = x.Id,
                        IsDemo = x.IsDemo,
                        ValidTo = x.ValidTo,
                        Created = x.CreatedDate,
                        SubscribedTo = x.SubscribedTo,
                        Type = (LicenseTypeEnum)(x.Type ?? 1),
                        User = new Common.Models.LicenserInfoModel()
                        {
                            Name = x.LicenseOwner.Name,
                            IsCompany = x.LicenseOwner.IsCompany,
                            Email = x.LicenseOwner.Email,
                            Phone = x.LicenseOwner.Phone,
                            ConactPerson = x.LicenseOwner.ContactPerson,
                            CompanyId = x.LicenseOwner.CompanyId,
                        },
                        Modules = x.LicenseModules.Select(m => (LicenseModulesEnum)m.ModuleId).ToList()
                    })
                    .ToList();
            }
        }


        public void Activate(LicenseModel license, string activationId)
        {
            using (var db = new LicenseDbEntities()) 
            {
                var result = db.Licenses.FirstOrDefault(x => x.Id == license.Id);
                if (result == null) 
                {
                    throw new Exception(string.Format("License not found with Id: {0}", license.Id));
                }

                result.LicenseActivations.Add(new LicenseActivation() 
                {
                    ComputerId =  license.Type == LicenseTypeEnum.PerComputer ? activationId : string.Empty,
                    UserId = license.Type == LicenseTypeEnum.PerUser ? activationId : string.Empty,
                    ComputerCount = 1
                });

                db.SaveChanges();
            }
        }
    }
}
