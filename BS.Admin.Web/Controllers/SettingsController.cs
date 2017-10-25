using BS.Common;
using BS.LicenseServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BS.Admin.Web.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ILicenseService _service;

        public SettingsController()
            :this(new LicenseService())
        {
        }

        public SettingsController(ILicenseService service)
        {
            _service = service;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IPs()
        {
            return View();
        }

    }
}
