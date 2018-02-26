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
                        db.LicenseVariables.Add(new LicenseVariable()
                        {
                            LicenseId = licenseGuid,
                            VariableId = variable.Id,
                            Value = v.Value != null ? v.Value.ToString() : string.Empty
                        });
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

        public List<LicenseVariableModel> GetVariables(string licenseId)
        {
            using (var db = new LicenseDbEntities())
            {
                return db.LicenseVariables.Select(x => new LicenseVariableModel()
                {
                    Id = x.Id,
                    LicenseId = x.LicenseId,
                    Name = x.lu_LicenseVariables.Name,
                    Value = x.Value
                }).ToList<LicenseVariableModel>();
            }
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
