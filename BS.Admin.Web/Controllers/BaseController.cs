using BS.Common;
using BS.Common.Interfaces;
using BS.LicenseServer.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace BS.Admin.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly ILicenseService _licenseService;
        protected readonly IUserService _userService;

        protected static ILogger _logger = LogManager.GetCurrentClassLogger();

        public BaseController()
            :this(new LicenseService(WebSecurity.Initialized ? (int?)WebSecurity.CurrentUserId : null), new UserService())
        {
        }

        public BaseController(ILicenseService licenseService, IUserService userService)
        {
            _licenseService = licenseService;
            _userService = userService;
        }

        public ActionResult Success(int? tabIndex = null)
        {
            ViewBag.Message = "Успешно създаден!";
            return RedirectToAction("Index", new { tabIndex = tabIndex ?? 0 });
        }

        public ActionResult Cancel(int? tabIndex = null)
        {
            return RedirectToAction("Index", new { tabIndex = tabIndex ?? 0 });
        }
    }
}
