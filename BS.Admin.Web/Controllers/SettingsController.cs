using BS.Common;
using BS.LicenseServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BS.Admin.Web.Controllers
{
    public class SettingsController : BaseController
    {
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
