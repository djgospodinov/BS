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

    public class BaseSortedCollection
    {
        public SortedLicenseEnum? SortExpression { get; set; }

        public bool Asc { get; set; }

        public object IsAscending(int sort)
        {
            if (SortExpression.HasValue && SortExpression.Value == (SortedLicenseEnum)sort)
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
                switch (SortExpression.Value)
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
        public List<LicenserInfoModel> Licenses { get; set; }

        public override void Sort()
        {
            if (SortExpression.HasValue)
            {
                switch (SortExpression.Value)
                {
                    case SortedLicenseEnum.Id:
                        Licenses = Asc
                            ? Licenses.OrderBy(x => x.Id).ToList()
                            : Licenses.OrderByDescending(x => x.Id).ToList();
                        break;
                    //case SortedLicenseEnum.ValidTo:
                    //    Licenses = Asc
                    //        ? Licenses.OrderBy(x => x.ValidTo).ToList()
                    //        : Licenses.OrderByDescending(x => x.ValidTo).ToList();
                    //    break;
                    //case SortedLicenseEnum.IsDemo:
                    //    Licenses = Asc
                    //        ? Licenses.OrderBy(x => x.IsDemo).ToList()
                    //        : Licenses.OrderByDescending(x => x.IsDemo).ToList();
                    //    break;
                    //case SortedLicenseEnum.User:
                    //    Licenses = Asc
                    //        ? Licenses.OrderBy(x => x.User.Name).ToList()
                    //        : Licenses.OrderByDescending(x => x.User.Name).ToList();
                    //    break;
                    //case SortedLicenseEnum.Created:
                    //    Licenses = Asc
                    //        ? Licenses.OrderBy(x => x.Created).ToList()
                    //        : Licenses.OrderByDescending(x => x.Created).ToList();
                    //    break;
                    //case SortedLicenseEnum.Active:
                    //    Licenses = Asc
                    //        ? Licenses.OrderBy(x => x.IsActivated).ToList()
                    //        : Licenses.OrderByDescending(x => x.IsActivated).ToList();
                    //    break;
                    //case SortedLicenseEnum.Enabled:
                    //    Licenses = Asc
                    //        ? Licenses.OrderBy(x => x.Enabled).ToList()
                    //        : Licenses.OrderByDescending(x => x.Enabled).ToList();
                    //    break;
                    //case SortedLicenseEnum.Type:
                    //    Licenses = Asc
                    //        ? Licenses.OrderBy(x => x.Type).ToList()
                    //        : Licenses.OrderByDescending(x => x.Type).ToList();
                    //    break;
                }
            }
        }
    }
}