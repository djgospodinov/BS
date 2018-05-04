using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace BS.Api.Filters
{
    public class DecryptContentAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Content.Headers.ContentType.MediaType.Contains("bson"))
            {
                actionContext.Request.Content = DecryptContect(actionContext.Request.Content);
            }
        }

        private HttpContent DecryptContect(HttpContent content)
        {
            var result = content.ReadAsStringAsync().Result;

            return new StringContent(Base64Helper.FromBase64String(result), Encoding.UTF8, "application/json");
        }
    }
}