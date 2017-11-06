using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS.Common.Models;
using BS.LicenseServer.Db;
using System.Timers;
using BS.Common.Interfaces;

namespace BS.LicenseServer.Services
{
    public class IpFilterService : IIpFilterService
    {
        public IpAddressElement Get(int id)
        {
            using (var db = new LicenseDbEntities())
            {
                return db.IpFilters
                    .Where(x => x.Id == id)
                    .Select(x => new IpAddressElement()
                {
                    Id = x.Id,
                    Address = x.Address.Trim(),
                    Denied = x.Denied
                }).FirstOrDefault();
            }
        }

        public List<IpAddressElement> GetAll()
        {
            using (var db = new LicenseDbEntities())
            {
                return db.IpFilters.Select(x => new IpAddressElement()
                {
                    Id = x.Id,
                    Address = x.Address.Trim(),
                    Denied = x.Denied
                }).ToList();
            }
        }

        public bool UseIpFiltering()
        {
            using (var db = new LicenseDbEntities())
            {
                var row = db.Settings.FirstOrDefault();
                return row != null ? row.UseIPFilter : false;
            }
        }


        public bool Add(IpAddressElement ipAddressElement)
        {
            using (var db = new LicenseDbEntities())
            {
                if (db.IpFilters.Any(x => x.Address == ipAddressElement.Address))
                    return false;

                db.IpFilters.Add(new IpFilter() 
                {
                    Address = ipAddressElement.Address.Trim(),
                    Denied = ipAddressElement.Denied
                });

                db.SaveChanges();
            }

            return true;
        }

        public bool Edit(int id, IpAddressElement ipAddressElement)
        {
            using (var db = new LicenseDbEntities())
            {
                if (db.IpFilters.Any(x => x.Address == ipAddressElement.Address && x.Id != id))
                    return false;

                var result = db.IpFilters.FirstOrDefault(x => x.Id == id);
                if (result != null)
                {
                    result.Address = ipAddressElement.Address.Trim();
                    result.Denied = ipAddressElement.Denied;

                    db.SaveChanges();
                }
            }

            return true;
        }

        public bool Delete(int id)
        {
            using (var db = new LicenseDbEntities())
            {
                var result = db.IpFilters.FirstOrDefault(x => x.Id == id);
                if (result != null)
                {
                    db.IpFilters.Remove(result);
                }

                db.SaveChanges();
            }

            return true;
        }


        public void SetUseIpFiltering(bool useRestriction)
        {
            using (var db = new LicenseDbEntities())
            {
                var result = db.Settings.FirstOrDefault();
                if (result != null)
                {
                    result.UseIPFilter = useRestriction;
                }
                else 
                {
                    db.Settings.Add(new Setting() 
                    {
                        UseIPFilter = useRestriction
                    });
                }

                db.SaveChanges();
            }
        }
    }
}
