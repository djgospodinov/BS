using BS.Admin.Web.Models;
using BS.Common.Interfaces;
using BS.Common.Models;
using BS.LicenseServer.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BS.Admin.Web.Controllers
{
    public class UserLicenseController : BaseController
    {
        [HttpGet]
        public ActionResult Index(int page = 1, SortedLicenseEnum? sort = null, bool asc = true)
        {
            int recordsPerPage = 10;
            List<LicenserInfoModel> dbModel = _userService.GetAll();

            int records = dbModel.Count;
            int pageCount = (records + recordsPerPage - 1) / recordsPerPage;
            if (page > pageCount)
            {
                page = 1;
            }

            var pages = new string[pageCount];
            int index = 0;
            foreach (var p in pages)
            {
                pages[index++] = index.ToString();
            }
            ViewBag.Pages = pages;
            ViewBag.PageIndex = page;

            var result = new UserLicenseSortedCollection()
            {
                SortExpression = sort.HasValue ? (int?)sort.Value : null,
                Asc = asc,
                Users = dbModel
            };
            result.Sort();

            result.Users = result.Users
                .Skip((page - 1) * recordsPerPage)
                .Take(recordsPerPage)
                .ToList();

            return View(result);
        }

        public ActionResult UserLicense(int id) 
        {
            var user = _userService.Get(id);

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
            try 
            {
                if (ModelState.IsValid)
                {
                    var result = _userService.Create(model.ToDbModel());
                    
                    return SuccessResult();
                }
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
            }

            ViewBag.ErrorMessage = "Възникна грешка";
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var result = _userService.Get(id);
            if (result != null) 
            {
                return View(new CreateLicenseOwnerModel(result));
            }

            return Content("Потребителят не е намерен!");
        }

        [HttpPost]
        public ActionResult Edit(CreateLicenseOwnerModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _userService.Update(model.Id, model.ToDbModel());
                    if (result) 
                    {
                        return SuccessResult();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
            }

            ViewBag.ErrorMessage = "Възникна грешка";
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var result = _userService.Get(id);
            if (result != null)
            {
                return View(new CreateLicenseOwnerModel(result));
            }

            return Content("Потребителят не е намерен!");
        }
    }
}
