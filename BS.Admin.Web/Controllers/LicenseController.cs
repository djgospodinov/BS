using BS.Common;
using BS.LicenseServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BS.Admin.Web.Controllers
{
    //[Authorize]
    public class LicenseController : Controller
    {
        private readonly ILicenseService _service;

        public LicenseController()
            :this(new LicenseService())
        {
        }

        public LicenseController(ILicenseService service)
        {
            _service = service;
        }


        public ActionResult Index()
        {
            List<LicenseModel> result = _service.GetAll();

            return View(result);
        }

        public ActionResult Create() 
        {
            return View();
        }

        public ActionResult Users() 
        {
            return View();
        }
    }
}
