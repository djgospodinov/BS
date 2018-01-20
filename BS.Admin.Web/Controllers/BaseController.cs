using BS.Common;
using BS.Common.Interfaces;
using BS.LicenseServer.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BS.Admin.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly ILicenseService _licenseService;
        protected readonly IUserService _userService;

        protected static ILogger _logger = LogManager.GetCurrentClassLogger();

        public BaseController()
            :this(new LicenseService(), new UserService())
        {
        }

        public BaseController(ILicenseService licenseService, IUserService userService)
        {
            _licenseService = licenseService;
            _userService = userService;
        }

        protected ActionResult SuccessResult(int tabIndex)
        {
            ViewBag.Message = "Успешно създаден!";
            return RedirectToAction("Index", new { tabIndex = tabIndex });
        }
    }
}
