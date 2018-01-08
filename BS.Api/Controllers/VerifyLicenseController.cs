using BS.Api.Models;
using BS.Common;
using BS.Common.Models;
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
            :this(new LicenseService())
        {
        }

        public VerifyLicenseController(ILicenseService service)
        {
            _service = service;
        }

        /// <summary>
        /// Verifies if license is valid for the given activation rule and if so, 
        /// returns a message containing the encrypted license
        /// </summary>
        /// <param name="id">the id of the license</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/verifylicense/{id}/encrypted")]
        public IHttpActionResult GetEncrypted([FromUri]string id, [FromBody]VerifyLicenseRequest request)
        {
            try
            {
                var license = _service.Get(id);

                #region Validation
                if (license == null)
                {
                    return BadRequestWithError(ApiError.LicenseNotFound, 
                        string.Format("No such license with the given Id {0}.", id));
                }

                if (!license.Enabled) 
                {
                    return BadRequestWithError(ApiError.LicenseNotEnabled,
                        string.Format("LIcense with Id {0} has not been enabled.", id));
                }

                if (request == null
                    || string.IsNullOrEmpty(request.ActivationKey))
                {
                    return BadRequestWithError(ApiError.NoActivationKey, "No activation key supplied.");
                }

                if (!_service.CheckOrActivate(license, request.ActivationKey, request.ComputerName)) 
                {
                    return BadRequestWithError(ApiError.LicenseActivationFailed, "Cannot activate license.");
                }
                #endregion

                var serializedObject = JsonConvert.SerializeObject(license, new JsonSerializerSettings() 
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                var password = Guid.NewGuid().ToString().Replace("-", "");
                var result = StringCipher.Encrypt(serializedObject, password);

                return Ok<VerifyLicenseMessage>(new VerifyLicenseMessage()
                    {
                        Key = StringCipher.Encrypt(password, Constants.PublicKey),
                        License = result,
                        LicenseId = id
                    });
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex);

                return BadRequestWithError(ApiError.GeneralError);
            }
        }

        /// <summary>
        /// Verifies if license is valid for the given activation rule and if so, 
        /// returns a message containing the license information
        /// </summary>
        /// <param name="id">the id of the license</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/verifylicense/{id}")]
        public IHttpActionResult GetNotEncrypted([FromUri]string id, [FromBody]VerifyLicenseRequest request)
        {
            try
            {
                var license = _service.Get(id);

                #region Validation
                if (license == null)
                {
                    return BadRequestWithError(ApiError.LicenseNotFound,
                        string.Format("No such license with the given Id {0}.", id));
                }

                if (!license.Enabled)
                {
                    return BadRequestWithError(ApiError.LicenseNotEnabled,
                        string.Format("LIcense with Id {0} has not been enabled.", id));
                }

                if (request == null
                    || string.IsNullOrEmpty(request.ActivationKey))
                {
                    return BadRequestWithError(ApiError.NoActivationKey, "No activation key supplied.");
                }

                if (!_service.CheckOrActivate(license, request.ActivationKey, request.ComputerName))
                {
                    return BadRequestWithError(ApiError.LicenseActivationFailed, "Cannot activate license.");
                }
                #endregion

                return Ok<LicenseMessage>(new LicenseMessage(license));
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex);

                return BadRequestWithError(ApiError.GeneralError);
            }
        }
    }
}