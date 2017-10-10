using BS.Api.Common;
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
    [Authorize]
    public class LicenseController : AuthorizedController
    {
        private readonly ILicenseService _service = new LicenseService();

        // GET api/license/5
        /// <summary>
        /// Returns info for the licence by the given id
        /// </summary>
        /// <param name="id">the id of the license, it should guid</param>
        /// <returns></returns>
        public IHttpActionResult Get(string id)
        {
            try
            {
                var serializedObject = JsonConvert.SerializeObject(_service.Get(id));
                var result = StringCipher.Encrypt(serializedObject, "testpassword");

                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Returns info for the licence by the given id/bulstat
        /// </summary>
        /// <param name="id">this is the firm id, e.g. the bulstat</param>
        /// <returns></returns>
        public IHttpActionResult Licenses(string id)
        {
            try
            {
                var result = JsonConvert.SerializeObject(_service.GetByFilter(new LicenseFilterModel() 
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

        // POST api/license
        /// <summary>
        /// Creates a license
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody]LicenseModel model)
        {
            try
            {
                if (!ModelState.IsValid) 
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
                return BadRequest(ex.Message);
            }

            return BadRequest("Cannot create licence.");
        }

        //// POST api/license
        ///// <summary>
        ///// Creates many license
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult CreateMany([FromBody]List<LicenseModel> model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest("Cannot create licence.");
        //        }

        //        var result = string.Join(",", this.service.CreateMany(model));
        //        if (!string.IsNullOrEmpty(result))
        //        {
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //    return BadRequest("Cannot create licences.");
        //}

        // PUT api/license/5

        /// <summary>
        /// Edit license details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IHttpActionResult Put(string id, [FromBody]LicenseModel value)
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
                return BadRequest(ex.Message);
            }

            return BadRequest(string.Format("Cannot update license {0}.", id));
        }

        // DELETE api/license/5
        /// <summary>
        /// Marks a license as disabled
        /// </summary>
        /// <param name="id">the id of the license, it should guid</param>
        /// <returns></returns>
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
                return BadRequest(ex.Message);
            }

            return BadRequest(string.Format("Cannot delete license {0}.", id));
        }
    }
}
