using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BS.Common;
using BS.Common.Models;

namespace BS.Admin.Web.Models
{
    public enum SortedLicenseEnum 
    {
        Id = 1,
        ValidTo = 2,
        IsDemo = 3,
        User = 4,
        Created = 5,
        Active = 6,
        Enabled = 7,
        Type = 8
    }

    public enum SortedUserLicenseEnum
    {
        Id = 1,
        Name = 2,
        Email = 3,
        Phone = 4,
        IsDemo = 5,
        IsCompany = 6,
        CompanyId = 7,
    }

    public class BaseSortedCollection
    {
        public int? SortExpression { get; set; }

        public bool Asc { get; set; }

        public int Page { get; set; }

        public object IsAscending(int sort)
        {
            if (SortExpression.HasValue && SortExpression.Value == sort)
                return !Asc;

            return true;
        }

        public virtual void Sort() 
        {
        }
    }

    public class LicenseSortedCollection : BaseSortedCollection
    {
        public List<LicenseModel> Licenses { get; set; }

        public override void Sort()
        {
            if (SortExpression.HasValue)
            {
                var value = (SortedLicenseEnum)SortExpression.Value;
                switch (value)
                {
                    case SortedLicenseEnum.Id:
                        Licenses = Asc
                            ? Licenses.OrderBy(x => x.Id).ToList()
                            : Licenses.OrderByDescending(x => x.Id).ToList();
                        break;
                    case SortedLicenseEnum.ValidTo:
                        Licenses = Asc
                            ? Licenses.OrderBy(x => x.ValidTo).ToList()
                            : Licenses.OrderByDescending(x => x.ValidTo).ToList();
                        break;
                    case SortedLicenseEnum.IsDemo:
                        Licenses = Asc
                            ? Licenses.OrderBy(x => x.IsDemo).ToList()
                            : Licenses.OrderByDescending(x => x.IsDemo).ToList();
                        break;
                    case SortedLicenseEnum.User:
                        Licenses = Asc
                            ? Licenses.OrderBy(x => x.User.Name).ToList()
                            : Licenses.OrderByDescending(x => x.User.Name).ToList();
                        break;
                    case SortedLicenseEnum.Created:
                        Licenses = Asc
                            ? Licenses.OrderBy(x => x.Created).ToList()
                            : Licenses.OrderByDescending(x => x.Created).ToList();
                        break;
                    case SortedLicenseEnum.Active:
                        Licenses = Asc
                            ? Licenses.OrderBy(x => x.IsActivated).ToList()
                            : Licenses.OrderByDescending(x => x.IsActivated).ToList();
                        break;
                    case SortedLicenseEnum.Enabled:
                        Licenses = Asc
                            ? Licenses.OrderBy(x => x.Enabled).ToList()
                            : Licenses.OrderByDescending(x => x.Enabled).ToList();
                        break;
                    case SortedLicenseEnum.Type:
                        Licenses = Asc
                            ? Licenses.OrderBy(x => x.Type).ToList()
                            : Licenses.OrderByDescending(x => x.Type).ToList();
                        break;
                }
            }
        }
    }

    public class UserLicenseSortedCollection : BaseSortedCollection
    {
        public List<LicenserInfoModel> Users { get; set; }

        public override void Sort()
        {
            if (SortExpression.HasValue)
            {
                var value = (SortedUserLicenseEnum)SortExpression.Value;
                switch (value)
                {
                    case SortedUserLicenseEnum.Id:
                        Users = Asc
                            ? Users.OrderBy(x => x.Id).ToList()
                            : Users.OrderByDescending(x => x.Id).ToList();
                        break;
                    case SortedUserLicenseEnum.Name:
                        Users = Asc
                            ? Users.OrderBy(x => x.Name).ToList()
                            : Users.OrderByDescending(x => x.Name).ToList();
                        break;
                    case SortedUserLicenseEnum.IsDemo:
                        Users = Asc
                            ? Users.OrderBy(x => x.IsDemo).ToList()
                            : Users.OrderByDescending(x => x.IsDemo).ToList();
                        break;
                    case SortedUserLicenseEnum.Email:
                        Users = Asc
                            ? Users.OrderBy(x => x.Email).ToList()
                            : Users.OrderByDescending(x => x.Email).ToList();
                        break;
                    case SortedUserLicenseEnum.Phone:
                        Users = Asc
                            ? Users.OrderBy(x => x.Phone).ToList()
                            : Users.OrderByDescending(x => x.Phone).ToList();
                        break;
                    case SortedUserLicenseEnum.IsCompany:
                        Users = Asc
                            ? Users.OrderBy(x => x.IsCompany).ToList()
                            : Users.OrderByDescending(x => x.IsCompany).ToList();
                        break;
                    case SortedUserLicenseEnum.CompanyId:
                        Users = Asc
                            ? Users.OrderBy(x => x.CompanyId).ToList()
                            : Users.OrderByDescending(x => x.CompanyId).ToList();
                        break;
                }
            }
        }
    }
}