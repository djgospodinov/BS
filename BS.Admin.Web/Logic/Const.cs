using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Admin.Web
{
    public class Const
    {
        #region Roles
        public const string AdministratorRoleName = "Administrator";
        public const string SuperUserRoleName = "SuperUser";
        public const string NormalUserRoleName = "NormalUser";
        #endregion

        #region Permissions
        public const string CreateLicence = "CreateLicence";
        public const string EditLicence = "EditLicence";

        public const string CreateUserLicence = "CreateUserLicence";
        public const string EditUserLicence = "EditUserLicence";
        #endregion

        public const string ShortDatePattern = "dd/MM/yyyy";
    }
}