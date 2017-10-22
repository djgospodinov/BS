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
