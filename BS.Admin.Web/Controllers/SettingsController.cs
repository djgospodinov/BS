using BS.Common;
using BS.LicenseServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BS.Admin.Web.Models;

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
            var result = new IpRestrictionModel()
            {
                UseIpRestriction = false,
                IPs = IpResctrictionService.GetAll()
                    .Select(x => new IpModel()
                    {
                        IpAddress = x.Address,
                        IsDenied = x.Denied
                    }).ToList()
        };

            return View(result);
        }
    }
}
