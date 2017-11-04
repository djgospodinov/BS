using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace BS.Admin.Web
{
    public class RolesManager
    {
        private static Dictionary<string, List<string>> _rolesPermissions;

        static RolesManager()
        {
            _rolesPermissions = new Dictionary<string, List<string>>();

            _rolesPermissions.Add(Const.AdministratorRoleName,new List<string>()
            {
                Const.CreateLicence,
                Const.EditLicence,
                Const.CreateUserLicence,
                Const.EditUserLicence
            });

            _rolesPermissions.Add(Const.SuperUserRoleName,new List<string>()
            {
                Const.CreateLicence,
                Const.EditLicence,
                Const.CreateUserLicence,
                Const.EditUserLicence
            });

            _rolesPermissions.Add(Const.NormalUserRoleName,new List<string>());
        }

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

        public static List<string> GetUserPermissions(string userName)
        {
            try
            {
                var role = Roles.GetRolesForUser(userName).First();
                if (_rolesPermissions.ContainsKey(role))
                {
                    return _rolesPermissions[role];
                }
            }
            catch 
            {
            }

            return null;
        }
    }
}