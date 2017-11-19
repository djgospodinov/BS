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
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(int page = 1, SortedLicenseEnum? sort = null, bool asc = false, 
            int? userId = null) 
        {
            int recordsPerPage = 10;
            List<LicenseModel> dbModel = _licenseService.GetAll();

            var result = new LicenseSortedCollection()
            {
                SortExpression = sort.HasValue ? (int?)sort.Value : (int)SortedLicenseEnum.Created,
                Asc = asc,
                Licenses = dbModel.Where(x => !userId.HasValue || x.User.Id == userId)
                    .ToList()
            };
            result.Sort();

            result.Page = Paging(page, recordsPerPage, result.Licenses.Count);

            result.Licenses = result.Licenses
                .Skip((page - 1) * recordsPerPage)
                .Take(recordsPerPage)
                .ToList();

            ViewBag.ShowUser = !userId.HasValue;

            return PartialView("_Licenses", result);
        }

        [HttpGet]
        public JsonResult Data(LicenseFilterGridModel filter) 
        {
            var dbModel = _licenseService.GetAll()
                .Where(x => !filter.Тype.HasValue || (int)x.Type == filter.Тype.Value)
                .ToList();
            
            if (!string.IsNullOrEmpty(filter.SortField)) 
            {
                bool asc = filter.SortOrder.ToLower() == "asc";
                switch (filter.SortField.ToLower()) 
                {
                    case "type":
                        dbModel = asc
                            ? dbModel.OrderBy(x => x.Type).ToList()
                            : dbModel.OrderByDescending(x => x.Type).ToList();
                        break;
                }
            }

            var data = dbModel
                .Skip((filter.PageIndex - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new 
            {
                Id = string.Format("{0}...", x.Id.ToString().Substring(0, 20)),
                ValidTo = x.ValidTo.ToShortDateString(),
                Demo = x.IsDemo,
                Created = x.Created.ToShortDateString(),
                Activated = x.IsActivated,
                Enabled = x.Enabled,
                Type = (int)x.Type
            }).ToArray();

            var dataResult = new {
                data = data,
                itemsCount = dbModel.Count
            };

            return Json(dataResult, JsonRequestBehavior.AllowGet);
        }

        private int Paging(int page, int recordsPerPage, int recordsCount)
        {
            int pageCount = (recordsCount + recordsPerPage - 1) / recordsPerPage;
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
            return page;
        }

        [HttpGet]
        [AuthorizeUser(AccessLevel = Const.CreateLicence)]
        public ActionResult Create() 
        {
            return View(new CreateLicenseModel() 
            { 
                ValidTo = DateTime.Now.AddMonths(1),
                SubscribedTo = DateTime.Now.AddMonths(1)
            });
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
