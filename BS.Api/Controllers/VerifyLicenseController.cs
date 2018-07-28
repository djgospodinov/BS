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
    public class VerifyLicenseController : BaseController
    {
        private static readonly bool AllowedTestRequests = Convert.ToBoolean(ConfigurationManager.AppSettings["AllowTestRequests"]);

        private readonly ILicenseService _service;

        public VerifyLicenseController()
            :this(new LicenseService(null))
        {
        }

        public VerifyLicenseController(ILicenseService service)
        {
            _service = service;
        }

        /// <summary>
        /// Прави проверка дали дадена комбинация от лиценз и ключ за активиране е валидна,
        /// При възможност активира лиценз, ако има свободни позиции за активация
        /// </summary>
        /// <param name="id">the id of the license</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/verifylicense/{id}")]
        public IHttpActionResult Post([FromUri]string id, [FromBody]VerifyLicenseRequest request)
        {
            try
            {
                var license = _service.Get(id);

                #region Validation
                if (license == null)
                {
                    return BadRequestWithError(ApiErrorEnum.LicenseNotFound,
                        string.Format("No such license with the given Id {0}.", id));
                }

                if (!license.Enabled)
                {
                    return BadRequestWithError(ApiErrorEnum.LicenseNotEnabled,
                        string.Format("LIcense with Id {0} has not been enabled.", id));
                }

                if (request == null
                    || string.IsNullOrEmpty(request.ActivationKey))
                {
                    return BadRequestWithError(ApiErrorEnum.NoActivationKey, "No activation key supplied.");
                }

                if (!_service.CheckOrActivate(license, request.ActivationKey, request.ComputerName))
                {
                    return BadRequestWithError(ApiErrorEnum.LicenseActivationFailed, "Cannot activate license.");
                }
                #endregion

                return Ok(new LicenseMessage(license));
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex);

                return BadRequestWithError(ApiErrorEnum.GeneralError);
            }
        }

        /// <summary>
        /// Прави проверка дали дадена комбинация от лиценз и ключ за активиране е валидна,
        /// При възможност активира лиценз, ако има свободни позиции за активация
        /// </summary>
        /// <param name="id">the id of the license</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/license/{id}/activate")]
        public IHttpActionResult Activate([FromUri]string id, [FromBody]VerifyLicenseRequestEx request)
        {
            try
            {
                var license = _service.Get(id);

                #region Validation
                if (license == null)
                {
                    return BadRequestWithError(ApiErrorEnum.LicenseNotFound,
                        string.Format("No such license with the given Id {0}.", id));
                }

                if (!license.Enabled)
                {
                    return BadRequestWithError(ApiErrorEnum.LicenseNotEnabled,
                        string.Format("LIcense with Id {0} has not been enabled.", id));
                }

                if (request == null
                    || string.IsNullOrEmpty(request.ActivationKey))
                {
                    return BadRequestWithError(ApiErrorEnum.NoActivationKey, "No activation key supplied.");
                }

                if (!_service.Activate(license.Id, new LicenseActivateModel()
                {
                    ActivationKey = request.ActivationKey,
                    ComputerName = request.ComputerName,
                    ServerName = request.ServerName
                }))
                {
                    return BadRequestWithError(ApiErrorEnum.LicenseActivationFailed, "Cannot activate license.");
                }
                #endregion

                return Ok(new ActivateLicenseMessage()
                {
                    LicenseId = license.Id,
                    ActivationKey = request.ActivationKey,
                    ComputerName = request.ComputerName,
                    ServerName = request.ServerName,
                    Activated = true
                });
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex);

                return BadRequestWithError(ApiErrorEnum.GeneralError);
            }
        }
    }
}