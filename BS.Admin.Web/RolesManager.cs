using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace BS.Admin.Web
{
    public class RolesManager
    {
        public static bool IsAdministrator(string userName) 
        {
            return Roles.IsUserInRole(userName, BS.Admin.Web.Const.AdministratorRoleName);
        }

        public static bool CanCreateLicense(string userName)
        {
            return Roles.IsUserInRole(userName, BS.Admin.Web.Const.AdministratorRoleName)
                || Roles.IsUserInRole(userName, BS.Admin.Web.Const.SuperUserRoleName);
        }

        //public static bool IsUserAdministrator(string userName)
        //{
        //    return Roles.IsUserInRole(userName, BS.Admin.Web.Const.AdministratorRoleName);
        //}
    }
}