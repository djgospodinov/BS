using BS.Common.Interfaces;
using BS.Common.Models;
using BS.LicenseServer.Db;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS.LicenseServer.Helper;

namespace BS.LicenseServer.Services
{
    public class UserService : IUserService
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        public Common.Models.LicenserInfoModel Get(int id)
        {
            using (var db = new LicenseDbEntities())
            {
                return DbHelper.FromDbModel(db.LicenseOwners.FirstOrDefault(x => x.Id == id));
            }
        }

        public Common.Models.LicenserInfoModel Get(string companyId)
        {
            throw new NotImplementedException();
        }

        public List<Common.Models.LicenserInfoModel> GetAll()
        {
            using (var db = new LicenseDbEntities())
            {
                return db.LicenseOwners
                    .Select(DbHelper.FromDbModel)
                    .ToList();
            }
        }

        public bool Create(LicenserInfoModel model)
        {
            try
            {
                using (var db = new LicenseDbEntities())
                {
                    var result = DbHelper.CreateDbModel(model);

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

        public bool Update(int id, LicenserInfoModel model)
        {
            try
            {
                using (var db = new LicenseDbEntities())
                {
                    var dbModel = db.LicenseOwners.FirstOrDefault(x => x.Id == id);
                    if (dbModel != null) 
                    {
                        var result = DbHelper.CreateDbModel(model, dbModel);

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
