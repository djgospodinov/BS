using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BS.Admin.Web.Filters
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public string AccessLevel { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext) && httpContext.User.Identity.IsAuthenticated;
            if (!isAuthorized)
            {
                return false;
            }

            var userPermissions = RolesManager.GetUserPermissions(httpContext.User.Identity.Name.ToString());
            if (userPermissions != null)
            {
                string permissions = string.Join("", userPermissions);
                return permissions.Contains(this.AccessLevel);
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Error",
                                action = "UnAuthorized"
                            })
                        );
        }
    }
}