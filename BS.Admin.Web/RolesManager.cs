using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace BS.Admin.Web
{
    public class RolesManager
    {
        public static bool IsAdministrator(System.Security.Principal.IIdentity identity) 
        {
            if (!identity.IsAuthenticated) 
                return false;

            string userName = identity.Name;
            return Roles.IsUserInRole(userName, BS.Admin.Web.Const.AdministratorRoleName);
        }

        public static bool CanCreateLicense(System.Security.Principal.IIdentity identity)
        {
            if (!identity.IsAuthenticated)
                return false;

            string userName = identity.Name;
            return Roles.IsUserInRole(userName, BS.Admin.Web.Const.AdministratorRoleName)
                || Roles.IsUserInRole(userName, BS.Admin.Web.Const.SuperUserRoleName);
        }
    }
}