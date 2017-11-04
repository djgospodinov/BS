using BS.Common;
using BS.LicenseServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BS.Common.Models;
using BS.Admin.Web.Models;
using BS.Common.Interfaces;

namespace BS.Admin.Web.Controllers
{
    [Authorize]
    public class LicenseController : Controller
    {
        private readonly ILicenseService _licenseService;
        private readonly IUserService _userService;

        public LicenseController()
            :this(new LicenseService(), new UserService())
        {
        }

        public LicenseController(ILicenseService licenseService, IUserService userService)
        {
            _licenseService = licenseService;
            _userService = userService;
        }


        public ActionResult Index(int page = 1, SortedLicenseEnum? sort = null, bool asc = true)
        {
            int recordsPerPage = 10;
            List<LicenseModel> dbModel = _licenseService.GetAll();

            int records = dbModel.Count;
            int pageCount = (records + recordsPerPage - 1) / recordsPerPage;
            if (page > pageCount) 
            {
                page = 1;
            }

            var pages = new string[pageCount];
            int index = 0;
            foreach(var p in pages)
            {
                pages[index++] = index.ToString();
            }
            ViewBag.Pages = pages;
            ViewBag.PageIndex = page;

            var result = new LicenseSortedCollection() 
            {
                SortExpression = sort,
                Asc = asc,
                Licenses = dbModel
            };
            result.Sort();

            result.Licenses = result.Licenses
                .Skip((page - 1) * recordsPerPage)
                .Take(recordsPerPage)
                .ToList();

            return View(result);
        }

        [HttpGet]
        public ActionResult Create() 
        {
            if (!RolesManager.CanCreateLicense(this.User.Identity)) 
            {
                return RedirectToAction("UnAuthorized", "Error");
            }

            var model = new CreateLicenseModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateLicenseModel model)
        {
            try
            {
                if (!RolesManager.CanCreateLicense(this.User.Identity))
                {
                    return RedirectToAction("UnAuthorized", "Error");
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var dbModel = model.ToDbModel(_userService);
                var id = _licenseService.Create(dbModel);
                if (!string.IsNullOrEmpty(id))
                {
                    ViewBag.Message = "Успешно създаден!";

                    return RedirectToAction("Index");
                }
            }
            catch 
            {
            }

            ViewBag.ErrorMessage = "Възникна грешка!";
            return View(new CreateLicenseModel(model));
        }

        [HttpGet]
        public ActionResult Edit(Guid id) 
        {
            try
            {
                if (!RolesManager.CanCreateLicense(this.User.Identity))
                {
                    return RedirectToAction("UnAuthorized", "Error");
                }

                var dbModel = _licenseService.Get(id.ToString());
                if (dbModel != null)
                {
                    var result = new CreateLicenseModel(dbModel);
                    if (result != null)
                    {
                        return View(result);
                    }
                }
            }
            catch 
            {
            }

            return Content("License not found");
        }

        [HttpPost]
        public ActionResult Edit(CreateLicenseModel model) 
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (_licenseService.Update(model.Id.ToString(), model.ToDbModel(_userService)))
                {
                    return RedirectToAction("Index");
                }
            }
            catch 
            {

            }

            ViewBag.ErrorMessage = "Възникна грешка!";
            return View(new CreateLicenseModel(model));
        }

        public ActionResult Details(string id)
        {
            try
            {
                var dbModel = _licenseService.Get(id.ToString());
                if (dbModel != null)
                {
                    var result = new CreateLicenseModel(dbModel);
                    if (result != null)
                    {
                        return View(result);
                    }
                }
            }
            catch
            {
            }

            return Content("License not found");
        }
    }
}
