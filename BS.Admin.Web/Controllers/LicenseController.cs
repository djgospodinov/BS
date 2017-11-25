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

        [HttpGet]
        public JsonResult Data(LicenseFilterGridModel filter) 
        {
            var dbModel = _licenseService.GetAll();

            if (filter.UserId.HasValue) 
            {
                dbModel = dbModel.Where(x => x.User.Id == filter.UserId.Value)
                    .ToList();
            }

            dbModel = dbModel
                .Where(x => (filter.Type == 0 || (int)x.Type == filter.Type)
                    && (!filter.Demo.HasValue || x.IsDemo == filter.Demo.Value)
                    && (!filter.Activated.HasValue || x.IsActivated == filter.Activated.Value)
                    && (!filter.Enabled.HasValue || x.Enabled == filter.Enabled.Value)
                    && (string.IsNullOrEmpty(filter.UserName) || x.User.Name.StartsWith(filter.UserName, StringComparison.CurrentCultureIgnoreCase)))
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
                UserName = x.User.Name,
                Created = x.Created.ToShortDateString(),
                Activated = x.IsActivated,
                Enabled = x.Enabled,
                Type = (int)x.Type,
                EditUrl = RolesManager.CanCreateLicense(User.Identity) 
                    ? string.Format("../License/Edit/{0}", x.Id)
                    : string.Empty
            }).ToArray();

            var dataResult = new {
                data = data,
                itemsCount = dbModel.Count
            };

            return Json(dataResult, JsonRequestBehavior.AllowGet);
        }

        #region CRUD

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

        [HttpGet]
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

        [HttpGet]
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

        #endregion
    }
}
