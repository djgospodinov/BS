using BS.Common.Interfaces;
using BS.Common.Models;
using BS.LicenseServer.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.LicenseServer.Services
{
    public class VariablesService : IVariablesService
    {
        public bool CreateLookupVariable(string name, string type = null)
        {
            using (var db = new LicenseDbEntities())
            {
                var variable = db.lu_LicenseVariables.FirstOrDefault(x => x.Name == name);
                if (variable != null)
                {
                    return false;
                }

                var result = new lu_LicenseVariables() { Name = name, Type = type };
                db.lu_LicenseVariables.Add(result);
                db.SaveChanges();
            }

            return true;
        }

        public VariableModel GetLookupVariable(int id)
        {
            using (var db = new LicenseDbEntities())
            {
                var variable = db.lu_LicenseVariables.FirstOrDefault(x => x.Id == id);
                if (variable != null)
                {
                    return new VariableModel()
                    {
                        Id = variable.Id,
                        Name = variable.Name,
                        Type = variable.Type
                    };
                }
            }

            return null;
        }

        public void CreateVariables(string licenseId, Dictionary<string, object> values)
        {
            using (var db = new LicenseDbEntities())
            {
                var licenseGuid = Guid.Parse(licenseId);
                foreach (var v in values)
                {
                    var variable = db.lu_LicenseVariables.FirstOrDefault(x => x.Name == v.Key);
                    if (variable != null)
                    {
                        int intValue;
                        if (variable.Type == ((int)VariableTypeEnum.Integer).ToString() && v.Value != null
                            && !int.TryParse(v.Value.ToString(), out intValue))
                        {
                            continue;
                        }

                        var licenseVariable = db.LicenseVariables.FirstOrDefault(x => x.lu_LicenseVariables.Id == variable.Id
                            && x.LicenseId == licenseGuid);
                        if (licenseVariable != null)
                        {
                            licenseVariable.Value = v.Value != null ? v.Value.ToString() : string.Empty;
                        }
                        else
                        {
                            db.LicenseVariables.Add(new LicenseVariable()
                            {
                                LicenseId = licenseGuid,
                                VariableId = variable.Id,
                                Value = v.Value != null ? v.Value.ToString() : string.Empty
                            });
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        public void DeleteVariables(string licenseId, List<string> variables)
        {
            using (var db = new LicenseDbEntities())
            {
                var licenseGuid = Guid.Parse(licenseId);
                foreach (var v in variables)
                {
                    var variable = db.LicenseVariables.FirstOrDefault(x => x.lu_LicenseVariables.Name == v);
                    if (variable != null)
                    {
                        db.LicenseVariables.Remove(variable);
                    }
                }

                db.SaveChanges();
            }
        }

        public List<LicenseVariableModel> GetLookupVariables()
        {
            using (var db = new LicenseDbEntities())
            {
                return db.lu_LicenseVariables
                    .Select(x => new LicenseVariableModel()
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList<LicenseVariableModel>();
            }
        }

        public List<LicenseVariableModel> GetVariables(string licenseId = null)
        {
            using (var db = new LicenseDbEntities())
            {
                var id = Guid.Parse(licenseId);
                return db.LicenseVariables
                    .Where(x => licenseId == null || x.LicenseId == id)
                    .Select(x => new LicenseVariableModel()
                {
                    Id = x.Id,
                    LicenseId = x.LicenseId,
                    Name = x.lu_LicenseVariables.Name,
                    Value = x.Value
                }).ToList<LicenseVariableModel>();
            }
        }

        public bool UpdateLookupVariable(VariableModel model)
        {
            using (var db = new LicenseDbEntities())
            {
                var variable = db.lu_LicenseVariables.FirstOrDefault(x => x.Id == model.Id);
                if (variable == null)
                {
                    return false;
                }

                db.lu_LicenseVariables.Add(new lu_LicenseVariables() { Name = model.Name, Type = model.Type });
                db.SaveChanges();
            }

            return true;
        }

        public void UpdateVariables(string licenseId, Dictionary<string, object> values)
        {
            using (var db = new LicenseDbEntities())
            {
                var licenseGuid = Guid.Parse(licenseId);
                foreach (var v in values)
                {
                    var variable = db.LicenseVariables.FirstOrDefault(x => x.lu_LicenseVariables.Name == v.Key);
                    if (variable != null)
                    {
                        variable.Value = v.Value != null ? v.Value.ToString() : string.Empty;
                    }
                }
                db.SaveChanges();
            }
        }
    }
}
