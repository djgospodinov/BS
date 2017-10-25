using BS.Common;
using BS.LicenseServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BS.Common.Models;

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


        public ActionResult Index(int page = 1)
        {
            int recordsPerPage = 10;
            List<LicenseModel> result = _service.GetAll();

            int records = result.Count;
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

            return View(result
                .Skip((page - 1) * recordsPerPage)
                .Take(recordsPerPage)
                .ToList());
        }

        [HttpGet]
        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(LicenseModel model)
        {
            model.User = new LicenserInfoModel();
            model.User.CompanyId = "123";
            model.User.Name = "123";
            model.User.Email = "123";
            model.User.Phone = "123";
            model.User.ConactPerson = "123";

            model.Modules = new List<LicenseModulesEnum>();
            model.Modules.Add(LicenseModulesEnum.Accounting);

            //if (ModelState.IsValid)
            //{
                var id = _service.Create(model);
                if (!string.IsNullOrEmpty(id))
                {
                    ViewBag.Message = "Успешно създаден!";

                    var result = _service.Get(id);
                    return View(result);
                }
            //}

            ViewBag.Message = "Възникна грешка!";
            return View(model);
        }

        public ActionResult Users() 
        {
            return View();
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return Content("Not found.");
            }

            var result = _service.Get(id);
            if (result == null)
                throw new ArgumentException("License for id not found", id);

            return View(result);
        }
    }
}
