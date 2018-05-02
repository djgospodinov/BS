using BS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BS.Api.Handlers
{
    public class AuthorizationHeaderHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            IEnumerable<string> apiKeyHeaderValues = null;
            if (!request.RequestUri.AbsolutePath.Contains("api"))
            {
                return base.SendAsync(request, cancellationToken);
            }

            if (request.Headers.TryGetValues("X-ApiKey", out apiKeyHeaderValues))
            {
                var apiKeyHeaderValue = apiKeyHeaderValues.First();

                if (!string.IsNullOrEmpty(apiKeyHeaderValue) && apiKeyHeaderValue == Constants.ApiKeyValue)
                {
                    return base.SendAsync(request, cancellationToken);
                }
            }

            return Task.FromResult(request.CreateResponse(HttpStatusCode.Unauthorized));
        }
    }
}