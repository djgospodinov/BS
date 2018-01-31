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
                #region Sort
                bool asc = filter.SortOrder.ToUpper() == "ASC";
                switch (filter.SortField.ToUpper())
                {
                    case "ID":
                        dbModel = asc
                            ? dbModel.OrderBy(x => x.Id).ToList()
                            : dbModel.OrderByDescending(x => x.Id).ToList();
                        break;
                    case "VALIDTO":
                        dbModel = asc
                            ? dbModel.OrderBy(x => x.ValidTo).ToList()
                            : dbModel.OrderByDescending(x => x.ValidTo).ToList();
                        break;
                    case "DEMO":
                        dbModel = asc
                            ? dbModel.OrderBy(x => x.IsDemo).ToList()
                            : dbModel.OrderByDescending(x => x.IsDemo).ToList();
                        break;
                    case "USERNAME":
                        dbModel = asc
                            ? dbModel.OrderBy(x => x.User.Name).ToList()
                            : dbModel.OrderByDescending(x => x.User.Name).ToList();
                        break;
                    case "CREATED":
                        dbModel = asc
                            ? dbModel.OrderBy(x => x.Created).ToList()
                            : dbModel.OrderByDescending(x => x.Created).ToList();
                        break;
                    case "ACTIVATED":
                        dbModel = asc
                            ? dbModel.OrderBy(x => x.IsActivated).ToList()
                            : dbModel.OrderByDescending(x => x.IsActivated).ToList();
                        break;
                    case "ENABLED":
                        dbModel = asc
                            ? dbModel.OrderBy(x => x.Enabled).ToList()
                            : dbModel.OrderByDescending(x => x.Enabled).ToList();
                        break;
                    case "TYPE":
                        dbModel = asc
                            ? dbModel.OrderBy(x => x.Type).ToList()
                            : dbModel.OrderByDescending(x => x.Type).ToList();
                        break;
                }
                #endregion
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
                        : string.Empty,
                    DetailUrl = string.Format("../License/Details/{0}", x.Id)
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
                ValidTo = DateTime.Now.AddYears(10),
                SubscribedTo = DateTime.Now.AddMonths(12)
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
                    return Success();
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
                    return Success();
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
