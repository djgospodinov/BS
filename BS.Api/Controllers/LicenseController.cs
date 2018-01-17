using BS.Api.Models;
using BS.Api.Services;
using BS.Common;
using BS.Common.Models;
using BS.LicenseServer.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BS.Api.Controllers
{
    //[Authorize]
    public class LicenseController : BaseController
    {
        private readonly ILicenseService _service = new LicenseService();

        /// <summary>
        /// Returns info for the licence by the given license id
        /// </summary>
        /// <param name="id">this is the id of the license</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/license/{id}")]
        public IHttpActionResult Get(string id)
        {
            try
            {
                var result = _service.Get(id);
                
                return Ok<LicenseModel>(result);
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex);

                return BadRequestWithError(ApiErrorEnum.GeneralError);
            }
        }

        /// <summary>
        /// Returns license type by the given license id(PerComputer, PerUser, PerServer)
        /// </summary>
        /// <param name="id">license id</param>
        /// <returns>string: PerComputer, PerUser, PerServer</returns>
        [HttpGet]
        [Route("api/license/{id}/type")]
        public IHttpActionResult GetType(string id)
        {
            try
            {
                var result = _service.Get(id);

                return Ok<string>(result.Type.ToString());
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex);

                return BadRequestWithError(ApiErrorEnum.GeneralError);
            }
        }

        /// <summary>
        /// Returns information about all licenses for given bulstat/egn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/licenses/{id}")]
        public IHttpActionResult Licenses(string id)
        {
            try
            {
                var result = this._service.GetByFilter(new LicenseFilterModel()
                {
                    CompanyId = id
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex);

                return BadRequestWithError(ApiErrorEnum.GeneralError);
            }
        }

        /// <summary>
        /// Creates a license
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/license")]
        public IHttpActionResult Post([FromBody]LicenseModel model)
        {
            try
            {
                if (!ModelState.IsValid || (int)model.Type < 1 || (int)model.Type > 3) 
                {
                    return BadRequest("Cannot create licence.");
                }

                var result = _service.Create(model);
                if (!string.IsNullOrEmpty(result)) 
                {
                    return Ok(result);
                }
            }
            catch (Exception ex) 
            {
                _logger.Log(NLog.LogLevel.Error, ex);

                return BadRequestWithError(ApiErrorEnum.GeneralError);
            }

            return BadRequestWithError(ApiErrorEnum.LicenseCreateFailed, "Cannot create licence.");
        }

        /// <summary>
        /// Creates many license
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/licenses")]
        public IHttpActionResult CreateMany([FromBody]List<LicenseModel> model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequestWithError(ApiErrorEnum.LicenseCreateFailed, "Cannot create licence.");
                }

                var result = string.Join(",", _service.CreateMany(model));
                if (!string.IsNullOrEmpty(result))
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex);
                return BadRequest(ex.Message);
            }

            return BadRequestWithError(ApiErrorEnum.LicenseCreateFailed, "Cannot create licences.");
        }

        /// <summary>
        /// Edit license details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/license/{id}")]
        public IHttpActionResult Put(string id, [FromBody]UpdateLicenseModel value)
        {
            try
            {
                if (_service.Update(id, value))
                {
                    return Ok(id);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex);

                return BadRequestWithError(ApiErrorEnum.GeneralError);
            }

            return BadRequestWithError(ApiErrorEnum.LicenseUpdateFailed,
                string.Format("Cannot update license {0}.", id));
        }

        /// <summary>
        /// Marks a license as disabled
        /// </summary>
        /// <param name="id">the id of the license, it should guid</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/license/{id}")]
        public IHttpActionResult Delete(string id)
        {
            try
            {
                if (_service.Delete(id))
                {
                    return Ok(id);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex);

                return BadRequestWithError(ApiErrorEnum.GeneralError);
            }

            return BadRequestWithError(ApiErrorEnum.LicenseDeleteFailed, string.Format("Cannot delete license {0}.", id));
        }
    }
}
