using BS.Common;
using BS.Common.Interfaces;
using BS.LicenseServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BS.Api.Controllers
{
    public class VariableController : BaseController
    {
        private readonly IVariablesService _service = new VariablesService();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Variables/{licenseId}")]
        public IHttpActionResult Get(string licenseId)
        {
            try
            {
                var result = _service.GetVariables(licenseId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex);

                return BadRequestWithError(ApiErrorEnum.GeneralError);
            }
        }

        /// <summary>
        /// create
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Variables/{licenseId}")]
        public IHttpActionResult Post(string licenseId, Dictionary<string, object> values)
        {
            try
            {
                _service.CreateVariables(licenseId, values);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex);

                return BadRequestWithError(ApiErrorEnum.GeneralError);
            }
        }

        [HttpPut]
        [Route("api/Variable/{licenseId}")]
        public IHttpActionResult Put(string licenseId, Dictionary<string, object> values)
        {
            try
            {
                _service.UpdateVariables(licenseId, values);
                
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex);

                return BadRequestWithError(ApiErrorEnum.GeneralError);
            }
        }

        [HttpDelete]
        [Route("api/Variable/{licenseId}")]
        public IHttpActionResult Delete(string licenseId, List<string> values)
        {
            try
            {
                _service.DeleteVariables(licenseId, values);

                return Ok(string.Empty);
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex);

                return BadRequestWithError(ApiErrorEnum.GeneralError);
            }
        }
    }
}