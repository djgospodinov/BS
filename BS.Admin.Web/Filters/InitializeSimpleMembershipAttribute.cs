﻿using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using BS.Admin.Web.Models;
using System.Web.Security;
using System.Linq;

namespace BS.Admin.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        private static string  _defaultAdminName = "bsadmin";
        private static string _defaultAdminPass = "bsadmin!";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<UsersContext>(null);

                try
                {
                    using (var context = new UsersContext())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
                    InitializeDefaultUserAndRoles();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }

            private static void InitializeDefaultUserAndRoles()
            {
                if (WebSecurity.GetUserId(_defaultAdminName) <= 0)
                {
                    WebSecurity.CreateUserAndAccount(_defaultAdminName, _defaultAdminPass);
                }

                if (!Roles.RoleExists(Const.AdministratorRoleName))
                {
                    Roles.CreateRole(Const.AdministratorRoleName);
                }

                if (!Roles.RoleExists(Const.SuperUserRoleName))
                {
                    Roles.CreateRole(Const.SuperUserRoleName);
                }

                if (!Roles.RoleExists(Const.NormalUserRoleName))
                {
                    Roles.CreateRole(Const.NormalUserRoleName);
                }

                if (!Roles.GetRolesForUser(_defaultAdminName).Contains(Const.AdministratorRoleName))
                {
                    Roles.AddUsersToRole(new[] { _defaultAdminName }, Const.AdministratorRoleName);
                }
            }
        }
    }
}
