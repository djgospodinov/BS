using BS.Common.Models;
using BS.LicenseServer.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.LicenseServer.Cache
{
    public class LicenseLogCache : BaseCache
    {
        private List<LicenseLogModel> _licenseLogs;
        private Dictionary<short, string> _modules;
        private readonly object _lock = new object();

        public LicenseLogCache()
            : base(new TimeSpan(0, 0, 30))
        {
           
        }

        public List<LicenseLogModel> GetLogs()
        {
            return _licenseLogs.ToList();
        }

        protected override void Initiliaze()
        {
            lock (_lock)
            {
                using (var db = new LicenseDbEntities())
                {
                    _modules = _modules ?? db.lu_LicenseModules.ToDictionary(x => x.Id, y => y.Name);

                    _licenseLogs = db.LicensesLogs.Select(x => new LicenseLogModel()
                    {
                        Id = x.Id,
                        LicenseId = x.LicenseId,
                        IsDemo = x.IsDemo,
                        ChangedBy = x.ChangedBy,
                        Date = x.Date,
                        Old = x.Old,
                        New = x.New
                    }).ToList();
                }
                

                foreach (var v in _licenseLogs)
                {
                    v.Changes = CreateChanges(v.Old, v.New);
                }
            }
        }

        private Dictionary<string, LicenseLogChangeItem> CreateChanges(string oldString, string newString)
        {
            var oldObject = JsonConvert.DeserializeObject<License>(oldString);
            var newObject = JsonConvert.DeserializeObject<License>(newString);

            var result = new Dictionary<string, LicenseLogChangeItem>();

            if (oldObject != null && newObject != null)
            {
                Type type = typeof(License);
                List<string> ignoreList = new List<string>() { "LicenseActivations" };
                foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    if (!ignoreList.Contains(pi.Name))
                    {
                        switch (pi.Name)
                        {
                            case "LicenseModules":
                                bool changed = false;
                                var oldModules = oldObject.LicenseModules.Select(x => _modules[x.ModuleId])
                                    .ToList();
                                var newModules = newObject.LicenseModules.Select(x => _modules[x.ModuleId])
                                    .ToList();
                                if (oldModules.Count != newModules.Count)
                                {
                                    changed = true;
                                }
                                else
                                {
                                    foreach (var m in newModules)
                                    {
                                        if (!oldModules.Contains(m))
                                        {
                                            changed = true;
                                            break;
                                        }
                                    }

                                    foreach (var m in oldModules)
                                    {
                                        if (!newModules.Contains(m))
                                        {
                                            changed = true;
                                            break;
                                        }
                                    }
                                }

                                if (changed)
                                {
                                    result.Add(pi.Name, new LicenseLogChangeItem()
                                    {
                                        OldValue = string.Join(",", oldModules),
                                        NewValue = string.Join(",", newModules),
                                    });
                                }
                                break;
                            default:
                                object oldValue = type.GetProperty(pi.Name).GetValue(oldObject, null);
                                object newValue = type.GetProperty(pi.Name).GetValue(newObject, null);

                                if (oldValue != newValue && (oldValue == null || !oldValue.Equals(newValue)))
                                {
                                    result.Add(pi.Name, new LicenseLogChangeItem()
                                    {
                                        OldValue = oldValue,
                                        NewValue = newValue
                                    });
                                }
                                break;
                        }
                    }
                }
            }

            return result;
        }
    }
}
