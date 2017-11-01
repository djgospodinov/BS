using BS.Admin.Web.Models;
using BS.Common.Interfaces;
using BS.LicenseServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BS.Admin.Web.Controllers
{
    public class UserLicenseController : Controller
    {
        private readonly IUserService _service = new UserService();
        //
        // GET: /UserLicense/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult User(int id) 
        {
            var user = _service.Get(id);

            return View(user);
        }

        [HttpGet]
        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateLicenseOwnerModel model)
        {
            if (ModelState.IsValid) 
            {
                //var result = _service.Create(model);
            }

            return View(model);
        }
    }
}
