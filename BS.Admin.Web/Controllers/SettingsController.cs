﻿using BS.Common;
using BS.LicenseServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BS.Admin.Web.Models;
using BS.Common.Interfaces;
using NLog;
using BS.Common.Models;
using BS.Admin.Web.Db;

namespace BS.Admin.Web.Controllers
{
    public class SettingsController : BaseController
    {
        #region Initialize
        private readonly IIpFilterService _service;
        private readonly ApiLogService _apiLogService = new ApiLogService();
        private readonly IVariablesService _variableService = new VariablesService();

        public SettingsController()
            : this(new IpFilterService())
        {
        }

        public SettingsController(IIpFilterService service)
        {
            _service = service;
        }
        #endregion

        public ActionResult Index(int tabIndex = 0)
        {
            return View(tabIndex);
        }

        [HttpGet]
        public JsonResult ApiLogData(ApiLogFilterGridModel filter)
        {
            var dbModel = _apiLogService.GetLogs();
            var data = dbModel
                .Where(x => (string.IsNullOrEmpty(filter.RequestUri) || x.RequestUri.ToUpper().Contains(filter.RequestUri.ToUpper()))
                    && (string.IsNullOrEmpty(filter.AbsoluteUri) || x.AbsoluteUri.ToUpper().Contains(filter.AbsoluteUri.ToUpper()))
                    && (string.IsNullOrEmpty(filter.RequestMethod) || x.RequestMethod.ToUpper() == filter.RequestMethod.ToUpper())
                    && (string.IsNullOrEmpty(filter.RequestIpAddress) || x.RequestIpAddress.Contains(filter.RequestIpAddress))
                    && (!filter.ResponseStatusCode.HasValue || x.ResponseStatusCode == filter.ResponseStatusCode))
                .ToList();

            if (!string.IsNullOrEmpty(filter.SortField))
            {
                #region Sort
                bool asc = filter.SortOrder.ToUpper() == "ASC";
                switch (filter.SortField.ToUpper())
                {
                    case "REQUESTURI":
                        data = asc
                            ? data.OrderBy(x => x.RequestUri).ToList()
                            : data.OrderByDescending(x => x.RequestUri).ToList();
                        break;
                    case "ABSOLUTEURI":
                        data = asc
                            ? data.OrderBy(x => x.AbsoluteUri).ToList()
                            : data.OrderByDescending(x => x.AbsoluteUri).ToList();
                        break;
                    case "REQUESTMETHOD":
                        data = asc
                            ? data.OrderBy(x => x.RequestMethod).ToList()
                            : data.OrderByDescending(x => x.RequestMethod).ToList();
                        break;
                    case "REQUESTIPADDRESS":
                        data = asc
                            ? data.OrderBy(x => x.RequestIpAddress).ToList()
                            : data.OrderByDescending(x => x.RequestIpAddress).ToList();
                        break;
                    case "RESPONSETIMESTAMP":
                        data = asc
                            ? data.OrderBy(x => x.ResponseTimestamp).ToList()
                            : data.OrderByDescending(x => x.ResponseTimestamp).ToList();
                        break;
                    case "RESPONSESTATUSCODE":
                        data = asc
                            ? data.OrderBy(x => x.ResponseStatusCode).ToList()
                            : data.OrderByDescending(x => x.ResponseStatusCode).ToList();
                        break;
                    default:
                        data = asc
                            ? data.OrderBy(x => x.ResponseTimestamp).ToList()
                            : data.OrderByDescending(x => x.ResponseTimestamp).ToList();
                        break;
                }
                #endregion
            }
            else
            {
                data = data.OrderByDescending(x => x.ResponseTimestamp).ToList();
            }

            var dataResult = new
            {
                data = data
                    .Skip((filter.PageIndex - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(x => new
                    {
                        Id = x.Id,
                        RequestIpAddress = x.RequestIpAddress,//TODO: Add loggic to add/remove ip from IpFilters
                        RequestMethod = x.RequestMethod,
                        RequestTimestamp = x.RequestTimestamp,
                        RequestUri = x.RequestUri,
                        AbsoluteUri = x.AbsoluteUri,
                        ResponseStatusCode = x.ResponseStatusCode,
                        ResponseTimestamp = x.ResponseTimestamp.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                        DetailUrl = string.Format("../Settings/ApiLogEntry/{0}", x.Id)
                    }),
                itemsCount = data.Count
            };

            return Json(dataResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApiLogEntry(int id)
        {
            var model = _apiLogService.GetLogEntry(id);
            if (model == null)
            {
                throw new Exception("Not found!");
            }

            return View(model);
        }

        [HttpGet]
        public JsonResult LicenseLogData(LicenseLogFilterGridModel filter)
        {
            var dbModel = _apiLogService.GetLicenseLogs();

            using (var db = new BSAdminDbEntities())
            {
                foreach (var log in dbModel)
                {
                    log.ChangedByName = (db.UserProfiles.FirstOrDefault(up => up.UserId == log.ChangedBy)
                                        ?? new Db.UserProfile() { UserName = "BsApi" }).UserName;
                }
            }

            var data = dbModel
                .Where(x => (string.IsNullOrEmpty(filter.ChangedBy)
                        || x.ChangedByName.ToUpper().Contains(filter.ChangedBy.ToUpper()))
                     && (!filter.IsDemo.HasValue || x.IsDemo == filter.IsDemo))
                .ToList();

            if (!string.IsNullOrEmpty(filter.SortField))
            {
                #region Sort
                bool asc = filter.SortOrder.ToUpper() == "ASC";
                switch (filter.SortField.ToUpper())
                {
                    case "ID":
                        data = asc
                            ? data.OrderBy(x => x.Id).ToList()
                            : data.OrderByDescending(x => x.Id).ToList();
                        break;
                    case "LICENSEID":
                        data = asc
                            ? data.OrderBy(x => x.LicenseId).ToList()
                            : data.OrderByDescending(x => x.LicenseId).ToList();
                        break;
                    case "ISDEMO":
                        data = asc
                            ? data.OrderBy(x => x.IsDemo).ToList()
                            : data.OrderByDescending(x => x.IsDemo).ToList();
                        break;
                    case "CHANGEDBY":
                        data = asc
                            ? data.OrderBy(x => x.ChangedBy).ToList()
                            : data.OrderByDescending(x => x.ChangedBy).ToList();
                        break;
                    default:
                        data = asc
                            ? data.OrderBy(x => x.Date).ToList()
                            : data.OrderByDescending(x => x.Date).ToList();
                        break;
                }
                #endregion
            }
            else
            {
                data = data.OrderByDescending(x => x.Date).ToList();
            }


            var dataResult = new
            {
                data = data
                    .Skip((filter.PageIndex - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(x => new
                    {
                        Id = x.Id,
                        LicenseId = x.LicenseId,
                        IsDemo = x.IsDemo,
                        ChangedBy = x.ChangedByName,
                        Date = x.Date.ToString("dd/MM/yyyy HH:mm:ss"),
                        DetailUrl = string.Format("../Settings/LicenseLogEntry/{0}", x.Id)
                    }).ToList(),
                itemsCount = data.Count()
            };

            return Json(dataResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LicenseLogEntry(int id)
        {
            var model = _apiLogService.GetLicenseLog(id);
            using (var db = new BSAdminDbEntities())
            {
                model.ChangedByName = (db.UserProfiles.FirstOrDefault(up => up.UserId == model.ChangedBy)
                                    ?? new Db.UserProfile() { UserName = "BsApi" }).UserName;
            }

            return View(model);
        }

        #region Variables

        [HttpGet]
        public JsonResult VariablesData(LicenseLogFilterGridModel filter)
        {
            var dbModel = _variableService.GetLookupVariables();

            var data = dbModel
                //.Where(x => (string.IsNullOrEmpty(filter.ChangedBy)
                //        || x.ChangedByName.ToUpper().Contains(filter.ChangedBy.ToUpper()))
                //     && (!filter.IsDemo.HasValue || x.IsDemo == filter.IsDemo))
                .ToList();

            if (!string.IsNullOrEmpty(filter.SortField))
            {
                #region Sort
                bool asc = filter.SortOrder.ToUpper() == "ASC";
                //switch (filter.SortField.ToUpper())
                //{
                //    case "ID":
                //        data = asc
                //            ? data.OrderBy(x => x.Id).ToList()
                //            : data.OrderByDescending(x => x.Id).ToList();
                //        break;
                //    case "LICENSEID":
                //        data = asc
                //            ? data.OrderBy(x => x.LicenseId).ToList()
                //            : data.OrderByDescending(x => x.LicenseId).ToList();
                //        break;
                //    case "ISDEMO":
                //        data = asc
                //            ? data.OrderBy(x => x.IsDemo).ToList()
                //            : data.OrderByDescending(x => x.IsDemo).ToList();
                //        break;
                //    case "CHANGEDBY":
                //        data = asc
                //            ? data.OrderBy(x => x.ChangedBy).ToList()
                //            : data.OrderByDescending(x => x.ChangedBy).ToList();
                //        break;
                //    default:
                //        data = asc
                //            ? data.OrderBy(x => x.Date).ToList()
                //            : data.OrderByDescending(x => x.Date).ToList();
                //        break;
                //}
                #endregion
            }
            else
            {
                //data = data.OrderByDescending(x => x.Date).ToList();
            }


            var dataResult = new
            {
                data = data
                    .Skip((filter.PageIndex - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(x => new
                    {
                        Id = x.Id,
                        x.Name,
                        EditUrl = string.Format("../Settings/EditVariable/{0}", x.Id)
                    }).ToList(),
                itemsCount = data.Count()
            };

            return Json(dataResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreateVariable()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditVariable(int id)
        {
            var model = _variableService.GetLookupVariable(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditVariable(VariableModel model)
        {
            var result = model.Id > 0 
                ? _variableService.UpdateLookupVariable(model)
                : _variableService.CreateLookupVariable(model.Name, model.Type);

            return RedirectToAction("Index", new { tabIndex = 2 });
        }

        #endregion

        #region Not Used
        public ActionResult IPs(int page = 1)
        {
            var result = new IpRestrictionModel()
            {
                UseIpRestriction = _service.UseIpFiltering(),
                IPs = _service.GetAll()
                    .Select(x => new IpModel()
                    {
                        Id = x.Id,
                        IpAddress = x.Address,
                        IsDenied = x.Denied
                    }).ToList()
            };

            int recordsPerPage = 10;
            int records = result.IPs.Count;
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

            result.IPs = result.IPs
                .Skip((page - 1) * recordsPerPage)
                .Take(recordsPerPage)
                .ToList();

            return View(result);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(IpModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _service.Add(new IpAddressElement()
                    {
                        Address = model.IpAddress,
                        Denied = model.IsDenied
                    });

                    if (result)
                    {
                        return RedirectToAction("IPs");
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
        public ActionResult Edit(int id)
        {
            var result = _service.Get(id);
            return View(new IpModel()
            {
                Id = result.Id,
                IpAddress = result.Address,
                IsDenied = result.Denied
            });
        }

        [HttpPost]
        public ActionResult Edit(IpModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _service.Edit(model.Id,
                        new IpAddressElement()
                        {
                            Address = model.IpAddress,
                            Denied = model.IsDenied
                        });

                    if (result)
                    {
                        return RedirectToAction("IPs");
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

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var result = _service.Delete(id);

                if (result)
                {
                    return RedirectToAction("IPs");
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
            }

            return RedirectToAction("IPs");
        }

        [HttpPost]
        public ActionResult UseRestriction(bool useIpRestriction)
        {
            try
            {
                _service.SetUseIpFiltering(useIpRestriction);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);
            }

            return RedirectToAction("IPs");
        }
        #endregion
    }
}
