using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BS.Api.Filters;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Threading;
using System.ComponentModel;
using System.Web.Http.ModelBinding;
using BS.Api.Handlers;

namespace BS.Api.Controllers
{
    /// <summary>
    /// Api errors enum
    /// </summary>
    public enum ApiErrorEnum
    {
        /// <summary>
        /// Error when there is an unexpected exception
        /// </summary>
        [Description("Error when there is an unexpected exception")]
        GeneralError = 100,
        /// <summary>
        /// License not found in the License server DB
        /// </summary>
        [Description("License not found in the License server DB")]
        LicenseNotFound = 200,
        /// <summary>
        /// License is not enabled
        /// </summary>
        [Description("License is not enabled")]
        LicenseNotEnabled = 201,
        /// <summary>
        /// License cannot be activated
        /// </summary>
        [Description("License cannot be activated")]
        LicenseActivationFailed = 202,
        /// <summary>
        /// Cannot create a license
        /// </summary>
        [Description("Cannot create a license")]
        LicenseCreateFailed = 300,
        /// <summary>
        /// Cannot update a license
        /// </summary>
        [Description("Cannot update a license")]
        LicenseUpdateFailed = 301,
        /// <summary>
        /// Cannot delete a license
        /// </summary>
        [Description("Cannot delete a license")]
        LicenseDeleteFailed = 302,
        /// <summary>
        /// Activation key is missing
        /// </summary>
        [Description("Activation key is missing")]
        NoActivationKey = 400,
    }

    public class ApiErrorMessages
    {
        public const string BadRequest = "Api error: mallformed or incorrect request.";
    }

    //[DecryptContent]
    //[RequireHttps]
    [IpFilter]
    public class BaseController : ApiController
    {
        protected ILogger _logger = LogManager.GetCurrentClassLogger();

        protected IHttpActionResult BadRequestWithError(ApiErrorEnum error, string message = null)
        {
            var response = Request.CreateResponse(HttpStatusCode.BadRequest,
                new
                {
                    error = error,
                    message = message ?? ApiErrorMessages.BadRequest
                });

            return ResponseMessage(response);
        }

        protected IHttpActionResult BadRequestWithError(ApiErrorEnum generalError, ModelStateDictionary modelState)
        {
            var error = ModelState.Values.Where(x => x.Errors != null && x.Errors.Any()).First().Errors.First();
            var response = Request.CreateResponse(HttpStatusCode.BadRequest,
                new
                {
                    error = generalError,
                    message = !string.IsNullOrEmpty(error.ErrorMessage) ? error.ErrorMessage : error.Exception != null ? error.Exception.Message : ApiErrorMessages.BadRequest
                });

            return ResponseMessage(response);
        }
    }
}