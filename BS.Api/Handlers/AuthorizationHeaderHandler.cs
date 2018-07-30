using BS.Common;
using BS.Common.Interfaces;
using BS.LicenseServer.Services;
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
        private readonly IAuthorizationService _service;

        public AuthorizationHeaderHandler()
            : this(new AuthorizationService())
        {
        }

        public AuthorizationHeaderHandler(IAuthorizationService service)
        {
            _service = service;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.RequestUri.AbsolutePath.ToLower().StartsWith("/api/"))
            {
                return base.SendAsync(request, cancellationToken);
            }

            IEnumerable<string> apiKeyHeaderValues = null;
            if (request.Headers.TryGetValues("X-ApiKey", out apiKeyHeaderValues))
            {
                var apiKeyHeaderValue = apiKeyHeaderValues.First();

                if (!string.IsNullOrEmpty(apiKeyHeaderValue) 
                    && _service.Authorize(apiKeyHeaderValue))
                {
                    return base.SendAsync(request, cancellationToken);
                }
            }

            return Task.FromResult(request.CreateResponse(HttpStatusCode.Unauthorized));
        }
    }
}