﻿using BS.Api.Models;
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

        [HttpPost]
        [Route("api/verifylicense/{id}")]
        public IHttpActionResult Index(string id, LicenseActivation activator)
        {
            try
            {
                var license = this._service.Get(id);
                if (license == null)
                {
                    return BadRequest(string.Format("No such license with the given Id {0}.", id));
                }

                if (activator != null) 
                {
                    if (license.Type == LicenseType.PerComputer) 
                    {
                        if (string.IsNullOrEmpty(activator.ComputerId)) 
                        {
                            return BadRequest(string.Format("ComputerID must be provided for license with type PerComputer.", id));
                        }

                        if (!license.ActivationId.Equals(activator.ComputerId)) 
                        {
                            return BadRequest(string.Format("License is activated for another computer Id.", id));
                        }
                    }

                    if (license.Type == LicenseType.PerUser) 
                    {
                        if (string.IsNullOrEmpty(activator.UserId)) 
                        {
                            return BadRequest(string.Format("UserID must be provided for license with type PerUser.", id));
                        }

                        if (!license.ActivationId.Equals(activator.UserId))
                        {
                            return BadRequest(string.Format("License is activated for another user Id.", id));
                        }
                    }

                    if (string.IsNullOrEmpty(license.ActivationId) && activator != null) 
                    {
                        _service.Activate(license, license.Type == LicenseType.PerUser ? activator.UserId :
                            license.Type == LicenseType.PerComputer ? activator.ComputerId : string.Empty);
                    }
                }

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

                return BadRequest(ApiErrorMessages.BadRequest);
            }
        }

        [HttpPost]
        [Route("api/verifylicense/test/{id}")]
        public IHttpActionResult NoEncryption(string id)
        {
            try
            {
                if (!AllowedTestRequests) 
                {
                    return NotFound();
                }

                var result = this._service.Get(id);

                return Ok<LicenseModel>(result);
            }
            catch (Exception ex)
            {
                _logger.Log(NLog.LogLevel.Error, ex);

                return BadRequest(ApiErrorMessages.BadRequest);
            }
        }
	}
}