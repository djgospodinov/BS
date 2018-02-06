using BS.Common.Models;
using BS.LicenseServer.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS.Common;

namespace BS.LicenseServer.Cache
{
    public class LicenseLogCache : BaseCache
    {
        private List<LicenseLogModel> _licenseLogs;
        private Dictionary<short, string> _modules;
        private readonly object _lock = new object();

        public LicenseLogCache()
            : base(new TimeSpan(0, 0, 15))
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

        #region Helper methods
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
                        var fieldName = GetFieldName(pi.Name);
                        switch (pi.Name.ToLower())
                        {
                            case "licensemodules":
                                #region 
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
                                    result.Add(fieldName, new LicenseLogChangeItem()
                                    {
                                        OldValue = string.Join(",", oldModules),
                                        NewValue = string.Join(",", newModules),
                                    });
                                }
                                break;
                            #endregion
                            case "type":
                                #region
                                var oldTypeValue = Convert.ToInt32(type.GetProperty(pi.Name).GetValue(oldObject, null));
                                var newTypeValue = Convert.ToInt32(type.GetProperty(pi.Name).GetValue(newObject, null));

                                if (oldTypeValue != newTypeValue && (oldTypeValue == null || !oldTypeValue.Equals(newTypeValue)))
                                {
                                    result.Add(fieldName, new LicenseLogChangeItem()
                                    {
                                        OldValue = ((LicenseTypeEnum)oldTypeValue).Description(),
                                        NewValue = ((LicenseTypeEnum)newTypeValue).Description()
                                    });
                                }
                                break;
                            #endregion
                            case "licenseownerid":
                                object oldOwnerValue = type.GetProperty(pi.Name).GetValue(oldObject, null);
                                object newOwnerValue = type.GetProperty(pi.Name).GetValue(newObject, null);

                                if (oldOwnerValue != newOwnerValue && (oldOwnerValue == null || !oldOwnerValue.Equals(newOwnerValue)))
                                {
                                    using (var db = new LicenseDbEntities())
                                    {
                                        result.Add(fieldName, new LicenseLogChangeItem()
                                        {
                                            OldValue = db.LicenseOwners.First(x => x.Id == (int)oldOwnerValue).Name,
                                            NewValue = db.LicenseOwners.First(x => x.Id == (int)newOwnerValue).Name
                                        });
                                    }
                                }
                                break;
                            default:
                                object oldValue = type.GetProperty(pi.Name).GetValue(oldObject, null);
                                object newValue = type.GetProperty(pi.Name).GetValue(newObject, null);

                                if (oldValue != newValue && (oldValue == null || !oldValue.Equals(newValue)))
                                {
                                    result.Add(fieldName, new LicenseLogChangeItem()
                                    {
                                        OldValue = GetValue(oldValue),
                                        NewValue = GetValue(newValue)
                                    });
                                }
                                break;
                        }
                    }
                }
            }

            return result;
        }

        private static Dictionary<string, string> FieldNames = new Dictionary<string, string>()
        {
            { "enabled" , "Потвърден" },
            { "validto", "Валиден до" },
            { "subscribedto", "Абониран до" },
            { "type", "Вид"},
            { "workstationscount", "Брой компютри" },
            { "isdemo", "Демо"},
            { "licensemodules", "Модули"},
            { "isactivated", "Активиран"},
            { "licenseownerid", "Потребител/Фирма"}
        };

        private string GetFieldName(string name)
        {
            if (FieldNames.ContainsKey(name.ToLower()))
            {
                return FieldNames[name.ToLower()];
            }

            return name;
        }

        private object GetValue(object value)
        {
            var type = value.GetType();

            switch (type.Name.ToLower())
            {
                case "datetime":
                    return ((DateTime)value).ToString("dd/MM/yyyy HH:mm:ss");
                case "boolean":
                    return ((bool)value).ToBgString();
                default:
                    return value;
            }
        }
        #endregion
    }
}
