﻿using BS.Api.Common;
using BS.Common.Models;
using BS.LicenseServer.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.LicenseServer.Services
{
    public class LicenseService : ILicenseService
    {
        //private readonly _db = new DB();
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
                        User = new Common.Models.RealLicenserInfoModel()
                        {
                            Name = result.LicenseOwner.Name,
                            IsCompany = result.LicenseOwner.IsCompany,
                            Email = result.LicenseOwner.Email,
                            Phone = result.LicenseOwner.Phone,
                            ConactPerson = result.LicenseOwner.ContactPerson
                        },
                        Modules = result.LicenseModules.Select(x => (LicenseModulesEnum)x.ModuleId).ToList()
                    };
                }
            }

            return null;
        }

        public List<Api.Models.LicenseModel> GetByCriteria(LicenseFilterModel filter)
        {
            using (var db = new LicenseDbEntities())
            {
                var result = db.Licenses.Where(x => x.LicenseOwner.IsCompany == true
                    //&& x.LicenseOwner.FirmId == firmId //TODO: add to database
                    );

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
                            ConactPerson = x.LicenseOwner.ContactPerson
                        },
                        Modules = x.LicenseModules.Select(m => (LicenseModulesEnum)m.ModuleId).ToList()
                    })
                    .ToList();
            }
        }

        public string Create(Api.Models.LicenseModel result)
        {
            using (var db = new LicenseDbEntities())
            {
                var model = new License()
                {
                    Id = Guid.NewGuid(),
                    IsDemo = result.IsDemo,
                    ValidTo = result.ValidTo,
                    LicenseOwner = new LicenseOwner()
                    {
                        Name = result.User.Name,
                        IsCompany = result.User.IsCompany,
                        Email = result.User.Email,
                        Phone = result.User.Phone,
                        ContactPerson = result.User.ConactPerson
                    },
                    LicenseModules = result.Modules.Select(x => new LicenseModule() { ModuleId = (short)x }).ToList()
                };

                var created = db.Licenses.Add(model);
                db.SaveChanges();

                return created.Id.ToString();
            }
        }

        public bool Update(string id, Api.Models.LicenseModel model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
