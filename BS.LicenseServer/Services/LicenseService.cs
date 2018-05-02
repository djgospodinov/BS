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
using BS.LicenseServer.Helper;
using Newtonsoft.Json;
using BS.Common.Models.Requests;

namespace BS.LicenseServer.Services
{
    public class LicenseService : Service, ILicenseService
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        public LicenseService(int? userId) 
            : base(userId)
        {
        }

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
                        WorkstationsCount = result.WorkstationsCount,
                        User = DbHelper.FromDbModel(result.LicenseOwner),
                        LicenseModules = result.LicenseModules
                            .Select(x => (LicenseModulesEnum)x.ModuleId).ToList(),
                        Modules = result.LicenseModules
                            .Select(x => new LicenseModuleModel() 
                            {
                                Id = x.Id,
                                Code = x.lu_LicenseModules.Code,
                                ValidTo = x.ValidTo,
                                Type = (LicenseModulesEnum)x.lu_LicenseModules.Id
                            }).ToList(),
                        ActivationId = ((LicenseTypeEnum)result.Type) == LicenseTypeEnum.PerUser
                            ? activator != null ? activator.UserId : string.Empty
                            : activator != null ? activator.ComputerId : string.Empty,
                        IsActivated = activator != null
                    };
                }
            }

            return null;
        }

        public List<LicenseActivationModel> LicenseActivations(string id)
        {
            using (var db = new LicenseDbEntities())
            {
                var guid = Guid.Parse(id);
                var result = db.Licenses.FirstOrDefault(x => x.Id == guid);
                if (result != null)
                {

                    return result.LicenseActivations.Select(x => new LicenseActivationModel()
                    {
                        ComptuterId = x.ComputerId,
                        UserId = x.UserId,
                        PCName = x.ComputerName
                    }).ToList();
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
                        WorkstationsCount = x.WorkstationsCount,
                        Type = (LicenseTypeEnum)x.Type,
                        User = new Common.Models.LicenserInfoModel()
                        {
                            Name = x.LicenseOwner.Name,
                            IsCompany = x.LicenseOwner.IsCompany,
                            Email = x.LicenseOwner.Email,
                            Phone = x.LicenseOwner.Phone,
                            ContactPerson = x.LicenseOwner.ContactPerson,
                            CompanyId = x.LicenseOwner.CompanyId
                        },
                        LicenseModules = x.LicenseModules.Select(m => (LicenseModulesEnum)m.ModuleId).ToList(),
                        Modules = x.LicenseModules
                           .Select(m => new LicenseModuleModel()
                           {
                               Id = m.Id,
                               Code = m.lu_LicenseModules.Code,
                               ValidTo = m.ValidTo
                           }).ToList(),
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
                    var owner = db.LicenseOwners.FirstOrDefault(x => x.Id == model.User.Id);
                    if (owner == null)
                    {
                        owner = model.User.IsCompany 
                            ? db.LicenseOwners.FirstOrDefault(x => x.CompanyId == model.User.CompanyId)
                            : db.LicenseOwners.FirstOrDefault(x => x.EGN == model.User.EGN);
                    }

                    if (owner == null)
                    {
                        owner = new LicenseOwner()
                        {
                            Name = model.User.Name,
                            IsCompany = model.User.IsCompany,
                            Email = model.User.Email,
                            Phone = model.User.Phone,
                            ContactPerson = model.User.ContactPerson,
                            CompanyId = model.User.CompanyId,
                            EGN = model.User.EGN
                        };
                    }

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
                            extraInfo.ContactPerson = userInfo.ContactPerson;
                            extraInfo.AccountingPerson = userInfo.AccountingPerson;
                            extraInfo.VatRegistration = userInfo.VATRegistration;

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
                        LicenseModules = model.LicenseModules.Select(x => new LicenseModule()
                        {
                            ModuleId = (short)x,
                            ValidTo = model.ValidTo
                        }).ToList(),
                        Enabled = !model.IsDemo ? false : true,//the real license should be enabled, afterwards e.g. after it is payed
                        CreatedDate = DateTime.Now,
                        WorkstationsCount = model.Type == LicenseTypeEnum.PerUser ? 1 : model.WorkstationsCount.Value
                    };

                    var created = db.Licenses.Add(result);
                    db.SaveChanges();

                    var id = created.Id;

                    if (id != Guid.Empty)
                    {
                        LogLicenseChange(db, result.IsDemo,
                            null,
                            Serialize(result), id);
                    }

                    return id.ToString();
                }
            }
            catch (Exception ex)
            {
                var error = ((System.Data.Entity.Validation.DbEntityValidationException) ex).EntityValidationErrors;
                _logger.Log(LogLevel.Error, ex);

                throw;
            }
        }

        public bool Update(string id, UpdateLicenseModel model)
        {
            using (var db = new LicenseDbEntities())
            {
                var result = db.Licenses.FirstOrDefault(x => x.Id == new Guid(id));
                if (result != null)
                {
                    string beforeUpdate = Serialize(result);

                    result.ValidTo = model.ValidTo ?? result.ValidTo;
                    result.SubscribedTo = model.SubscribedTo ?? result.SubscribedTo;
                    result.IsDemo = model.IsDemo ?? result.IsDemo;
                    result.Enabled = model.Enabled ?? result.Enabled;
                    result.Type = (byte?)model.Type ?? result.Type;
                    result.WorkstationsCount = model.Type == LicenseTypeEnum.PerUser ? 1 : model.ComputerCount;

                    if (model.UserId != result.LicenseOwner.Id)
                    {
                        result.LicenseOwner = db.LicenseOwners.First(x => x.Id == model.UserId);
                    }

                    #region Modules
                    if (model.Modules != null)
                    {
                        var modulesIds = model.Modules.Select(x => (short)x)
                        .ToList();

                        foreach (var moduleId in modulesIds)
                        {
                            if (result.LicenseModules.FirstOrDefault(x => x.ModuleId == moduleId) == null)
                            {
                                result.LicenseModules.Add(new LicenseModule()
                                {
                                    ModuleId = moduleId,
                                    ValidTo = result.ValidTo
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
                    }
                    #endregion

                    db.SaveChanges();

                    LogLicenseChange(db, result.IsDemo,
                        beforeUpdate,
                        Serialize(result), result.Id);

                    return true;
                }
            }

            return false;
        }

        private static string Serialize(License result)
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new EFContractResolver();
            settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.Formatting = Formatting.Indented;

            var beforeUpdate = JsonConvert.SerializeObject(result, settings);
            return beforeUpdate;
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
                        Enabled = x.Enabled ?? false,
                        IsActivated = x.LicenseActivations.Any(),
                        ValidTo = x.ValidTo,
                        Created = x.CreatedDate,
                        SubscribedTo = x.SubscribedTo,
                        Type = (LicenseTypeEnum)(x.Type ?? 1),
                        User = DbHelper.FromDbModel(x.LicenseOwner),
                        LicenseModules = x.LicenseModules.Select(m => (LicenseModulesEnum)m.ModuleId).ToList()
                    })
                    .ToList();
            }
        }

        public bool CheckOrActivate(LicenseModel license, string activationKey, string computerName)
        {
            bool result = false;
            using (var db = new LicenseDbEntities()) 
            {
                var licenseDb = db.Licenses.FirstOrDefault(x => x.Id == license.Id);
                if (licenseDb == null)
                    return false;

                switch (license.Type) 
                {
                    case LicenseTypeEnum.PerComputer:
                        bool found = false;
                        foreach(var activation in licenseDb.LicenseActivations)
                        {
                            if (activation.ComputerId == activationKey)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (found)
                        {
                            result = true;
                        }
                        else
                        {
                            if (licenseDb.LicenseActivations.Count >= licenseDb.WorkstationsCount)
                            {
                                result = false;
                            }
                            else
                            {
                                licenseDb.LicenseActivations.Add(new LicenseActivation() { ComputerId = activationKey, ComputerName = computerName });
                                result = true;
                            }
                        }
                        break;
                    case LicenseTypeEnum.PerUser:
                        if(licenseDb.LicenseActivations.Count == 0)
                            licenseDb.LicenseActivations.Add(new LicenseActivation() { UserId = activationKey, ComputerName = computerName });

                        result = licenseDb.LicenseActivations.Any(x => x.UserId == activationKey);
                        break;
                    case LicenseTypeEnum.PerServer:
                        result = true;
                        break;
                }

                db.SaveChanges();
            }

            return result;
        }

        public bool Activate(Guid id, LicenseActivateModel model)
        {
            bool result = false;
            using (var db = new LicenseDbEntities())
            {
                var licenseDb = db.Licenses.FirstOrDefault(x => x.Id == id);
                if (licenseDb == null)
                    return false;
                var server = db.LicenseOwnerServers.FirstOrDefault(x => x.LicenseOwnerID == licenseDb.LicenseOwnerId
                    && x.ServerInstance == model.ServerName);
                if (server == null)
                {
                    server = new LicenseOwnerServer()
                    {
                        LicenseOwnerID = licenseDb.LicenseOwnerId,
                        ServerInstance = model.ServerName,
                        CreateDate = DateTime.Now
                    };
                    db.LicenseOwnerServers.Add(server);
                }

                switch ((LicenseTypeEnum)licenseDb.Type)
                {
                    case LicenseTypeEnum.PerComputer:
                       if (licenseDb.LicenseActivations.Count < licenseDb.WorkstationsCount)
                        {
                            licenseDb.LicenseActivations.Add(new LicenseActivation()
                            {
                                ComputerId = model.ActivationKey,
                                ComputerName = model.ComputerName,
                                LicenseOwnerServerId = server.LicenseOwnerID
                            });
                            result = true;
                        }
                        break;
                    case LicenseTypeEnum.PerUser:
                        if (licenseDb.LicenseActivations.Count == 0)
                        {
                            licenseDb.LicenseActivations.Add(new LicenseActivation()
                            {
                                UserId = model.ActivationKey,
                                ComputerName = model.ComputerName,
                                LicenseOwnerServerId = server.LicenseOwnerID
                            });
                        }
                        break;
                    case LicenseTypeEnum.PerServer:
                        result = true;
                        break;
                }

                db.SaveChanges();
            }

            return result;
        }

        public bool AddServer(AddServerRequest request)
        {
            try
            {
                bool result = false;
                using (var db = new LicenseDbEntities())
                {
                    var owner = db.LicenseOwners.FirstOrDefault(x => x.RegNom == request.RegNom);
                    if (owner != null)
                    {
                        foreach (var x in request.Servers)
                        {
                            var server = db.LicenseOwnerServers.FirstOrDefault(v => v.ServerInstance == x.ServerInstance && v.LicenseOwnerID == owner.Id);
                            if (server == null)
                            {
                                db.LicenseOwnerServers.Add(new LicenseOwnerServer()
                                {
                                    LicenseOwnerID = owner.Id,
                                    ServerInstance = x.ServerInstance,
                                    ServerIPAddress = x.ServerIPAddress,
                                    SendFromPC = request.ComputerName,
                                    SendFromPCIPAddress = request.ComputerIP,
                                    CreateDate = request.RequestDate,
                                    SystemUserName = request.SystemUserName
                                });
                            }
                            else
                            {
                                server.ServerIPAddress = x.ServerIPAddress;
                                server.SendFromPC = request.ComputerName;
                                server.SendFromPCIPAddress = request.ComputerIP;
                                server.CreateDate = request.RequestDate;
                                server.SystemUserName = request.SystemUserName;
                            }
                        }

                        db.SaveChanges();

                        return true;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                var error = ((System.Data.Entity.Validation.DbEntityValidationException)ex).EntityValidationErrors;
                _logger.Log(LogLevel.Error, ex);

                throw;
            }
        }

        #region Helper methods
        private void LogLicenseChange(LicenseDbEntities db, 
            bool isDemo,
            string oldObject,
            string newObject, 
            Guid id)
        {
            db.LicensesLogs.Add(new LicensesLog()
            {
                LicenseId = id,
                Date = DateTime.Now,
                IsDemo = isDemo,
                Old = oldObject,
                New = newObject,
                ChangedBy = UserId ?? 0
            });

            db.SaveChanges();
        }
        #endregion
    }
}
