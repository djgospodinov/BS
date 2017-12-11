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
        GeneralError = 1,

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

    //public class BadRequestWithErrorResult : IHttpActionResult
    //{
    //    private string _message;

    //    public BadRequestWithErrorResult(string message)
    //    {
    //        _message = message;
    //    }

    //    public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
    //    {
    //        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
    //        response.Content = new StringContent(_message);
    //        return Task.FromResult(response);
    //    }
    //}
}