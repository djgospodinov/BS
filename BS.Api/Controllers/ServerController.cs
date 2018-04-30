using BS.Api.Models;
using BS.Common;
using BS.Common.Models;
using BS.Common.Models.Requests;
using BS.LicenseServer.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace BS.Api.Controllers
{
    public class ServerController : BaseController
    {
        private readonly ILicenseService _service;

        public ServerController()
            :this(new LicenseService(null))
        {
        }

        public ServerController(ILicenseService service)
        {
            _service = service;
        }

        /// <summary>
        /// Добавя информация за сървърите на Мираж,с които работят клиентите
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/server")]
        public IHttpActionResult Post([FromBody]AddServerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequestWithError(ApiErrorEnum.GeneralError);
                }

                var result = _service.AddServer(request);

                return Ok(new AddServerResponse() { AddedServer = result });
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex);

                return BadRequestWithError(ApiErrorEnum.GeneralError);
            }
        }
    }
}