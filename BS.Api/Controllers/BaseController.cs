using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace BS.Api.Controllers
{
    public class ApiErrors 
    {
        public const string BadRequest = "Api error: mallformed or incorrect request.";
    }

    public class BaseController : ApiController
    {
        protected ILogger _logger = LogManager.GetCurrentClassLogger();
	}
}