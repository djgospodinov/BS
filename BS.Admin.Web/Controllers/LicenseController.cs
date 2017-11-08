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
using BS.Admin.Web.Filters;
using NLog;
using Newtonsoft.Json;

namespace BS.Admin.Web.Controllers
{
    public class LicenseController : BaseController
    {
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
                SortExpression = sort.HasValue ? (int?)sort.Value : (int)SortedLicenseEnum.Created,
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
        [AuthorizeUser(AccessLevel = Const.CreateLicence)]
        public ActionResult Create() 
        {
            return View(new CreateLicenseModel());
        }

        [HttpPost]
        [AuthorizeUser(AccessLevel = Const.CreateLicence)]
        public ActionResult Create(CreateLicenseModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var id = _licenseService.Create(model.ToDbModel(_userService));
                if (!string.IsNullOrEmpty(id))
                {
                    return SuccessResult();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
            }

            ViewBag.ErrorMessage = "Възникна грешка!";
            return View(new CreateLicenseModel(model));
        }

        [HttpGet]
        [AuthorizeUser(AccessLevel = Const.EditLicence)]
        public ActionResult Edit(Guid id) 
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
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
            }

            return Content("License not found");
        }

        [HttpPost]
        [AuthorizeUser(AccessLevel = Const.EditLicence)]
        public ActionResult Edit(CreateLicenseModel model) 
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (_licenseService.Update(model.Id.ToString(), model.ToUpdateDbModel(_userService)))
                {
                    return SuccessResult();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
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
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
            }

            return Content("License not found");
        }

        public ActionResult LicenseCode(string id)
        {
            try
            {
                var license = _licenseService.Get(id.ToString());

                var serializedObject = JsonConvert.SerializeObject(license, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                return PartialView(new LIcenseCodeModel() 
                {
                    Code = StringCipher.Encrypt(serializedObject, Constants.PublicKey)
                });
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
            }

            return Content("License not found");
        }
    }
}
