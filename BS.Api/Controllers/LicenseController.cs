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
        private readonly ILicenseService service = new LicenseService();

        // GET api/values/5
        public LicenseModel Get(string id)
        {
            return this.service.Get(id);
        }

        // POST api/values
        public string Post([FromBody]LicenseModel model)
        {
            return this.service.Create(model);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]LicenseModel value)
        {
        }

        // DELETE api/values/5
        public void Delete(string id)
        {
        }
    }
}
