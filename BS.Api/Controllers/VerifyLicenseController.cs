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

        [HttpPost]
        [Route("api/verifylicense/{id}")]
        public IHttpActionResult Index(string id)
        {
            try
            {
                var license = this._service.Get(id);
                if (license == null)
                {
                    return BadRequest(string.Format("No such license with the given Id {0}.", id));
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

                return BadRequest(ApiErrors.BadRequest);
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

                return BadRequest(ApiErrors.BadRequest);
            }
        }
	}
}