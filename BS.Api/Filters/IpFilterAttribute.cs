using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using BS.Api.Extensions;

namespace BS.Api.Filters
{
    public class IpFilterAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext context)
        {
            return context.Request.IsIpAllowed();
        }
    }
}