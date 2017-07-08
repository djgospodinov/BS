using BS.Api.Common;
using BS.Api.Models;
using BS.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BS.Api.Controllers
{
    public class LicenseController : BaseController
    {
        private readonly ILicenseService service = new DemoLicenseService();

        // GET api/values/5
        public IHttpActionResult Get(string id)
        {
            try
            {
                var result = this.service.Get(id);

                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/values
        public IHttpActionResult Post([FromBody]LicenseModel model)
        {
            try
            {
                var result = this.service.Create(model);
                if (!string.IsNullOrEmpty(result)) 
                {
                    return Ok(result);
                }
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

            return BadRequest("Cannot delete licence.");
        }

        // PUT api/values/5
        public IHttpActionResult Put(string id, [FromBody]LicenseModel value)
        {
            try
            {
                if (this.service.Update(id, value))
                {
                    return Ok(id);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest(string.Format("Cannot create update license {0}.", id));
        }

        // DELETE api/values/5
        public IHttpActionResult Delete(string id)
        {
            try
            {
                if (this.service.Delete(id))
                {
                    return Ok(id);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest(string.Format("Cannot delete license {0}.", id));
        }
    }
}
