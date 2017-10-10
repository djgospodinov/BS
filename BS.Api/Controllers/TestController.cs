using BS.Api.Common;
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
    public class TestController : ApiController
    {
        private static readonly bool AllowedTestRequests = Convert.ToBoolean(ConfigurationManager.AppSettings["AllowTestRequests"]);

        private readonly ILicenseService _service;

        public TestController()
            :this(new LicenseService())
        {
        }

        public TestController(ILicenseService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult Encrypted(string id)
        {
            try
            {
                if (!AllowedTestRequests) 
                    return NotFound();
                
                var serializedObject = JsonConvert.SerializeObject(this._service.Get(id));
                var result = StringCipher.Encrypt(serializedObject, "testpassword");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult NotEncrypted(string id)
        {
            try
            {
                if (!AllowedTestRequests)
                    return NotFound();

                var result = JsonConvert.SerializeObject(this._service.Get(id));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult Licenses(string id)
        {
            try
            {
                if (!AllowedTestRequests)
                    return NotFound();

                var result = JsonConvert.SerializeObject(this._service.GetByFilter(new LicenseFilterModel()
                {
                    CompanyId = id
                }));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
	}
}