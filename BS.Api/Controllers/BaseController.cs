using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using BS.Api.Filters;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Threading;

namespace BS.Api.Controllers
{
    public enum ApiError
    {
        /// <summary>
        /// Error when there is an unexpected exception
        /// </summary>
        GeneralError = 100,
        /// <summary>
        /// License not found in the License server DB
        /// </summary>
        LicenseNotFound = 200,
        /// <summary>
        /// License is not enabled
        /// </summary>
        LicenseNotEnabled = 201,
        /// <summary>
        /// License cannot be activated
        /// </summary>
        LicenseActivationFailed = 202,
        /// <summary>
        /// Could not create a license
        /// </summary>
        LicenseCreateFailed = 300,
        /// <summary>
        /// Could not update a license
        /// </summary>
        LicenseUpdateFailed = 301,
        /// <summary>
        /// Cannot delete a license
        /// </summary>
        LicenseDeleteFailed = 302,
        /// <summary>
        /// Activation key is not supplied
        /// </summary>
        NoActivationKey = 400,
    }

    public class ApiErrorMessages
    {
        public const string BadRequest = "Api error: mallformed or incorrect request.";
    }

    [IpFilter]
    public class BaseController : ApiController
    {
        protected ILogger _logger = LogManager.GetCurrentClassLogger();

        protected IHttpActionResult BadRequestWithError(ApiError error, string message = null)
        {
            var response = Request.CreateResponse(HttpStatusCode.BadRequest,
                new
                {
                    error = error,
                    message = message ?? ApiErrorMessages.BadRequest
                });

            return ResponseMessage(response);
        }
    }
}