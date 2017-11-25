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
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Data(UserLicenseFilterGridModel filter)
        {
            var dbModel = _userService.GetAll()
                .Where(x => x.IsDemo == filter.Demo
                    && (string.IsNullOrEmpty(filter.Name) || x.Name.StartsWith(filter.Name, StringComparison.CurrentCultureIgnoreCase))
                    && (string.IsNullOrEmpty(filter.Email) || x.Email.StartsWith(filter.Email, StringComparison.CurrentCultureIgnoreCase))
                    && (string.IsNullOrEmpty(filter.Phone) || x.Phone.StartsWith(filter.Phone, StringComparison.CurrentCultureIgnoreCase))
                    && (string.IsNullOrEmpty(filter.CompanyId) || x.CompanyId.StartsWith(filter.CompanyId, StringComparison.CurrentCultureIgnoreCase))
                    && (!filter.Company.HasValue || x.IsCompany == filter.Company))
                .ToList();

            if (!string.IsNullOrEmpty(filter.SortField))
            {
                bool asc = filter.SortOrder.ToLower() == "asc";
                switch (filter.SortField.ToLower())
                {
                    case "name":
                        dbModel = asc
                            ? dbModel.OrderBy(x => x.Name).ToList()
                            : dbModel.OrderByDescending(x => x.Name).ToList();
                        break;
                }
            }

            var data = dbModel
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new 
                { 
                    Id = x.Id,
                    Name = x.Name,
                    Demo = x.IsDemo,
                    Email = x.Email,
                    Phone = x.Phone,
                    Company = x.IsCompany,
                    CompanyId = x.CompanyId,
                    EditUrl = RolesManager.CanCreateLicense(User.Identity)
                        ? string.Format("../UserLicense/Edit/{0}", x.Id)
                        : string.Empty
                })
                .ToList();

            var result = new {
                data = data,
                itemsCount = dbModel.Count
            };

            return Json(result, JsonRequestBehavior.AllowGet);
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
