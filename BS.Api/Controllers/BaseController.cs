using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using BS.Api.Filters;

namespace BS.Api.Controllers
{
    public enum Errors
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
	}
}