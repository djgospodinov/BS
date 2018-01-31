using BS.Common;
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

namespace BS.Admin.Web.Controllers
{
    public class SettingsController : BaseController
    {
        private readonly IIpFilterService _service;
        private readonly ApiLogService _apiLogService = new ApiLogService();

        public SettingsController()
            : this(new IpFilterService())
        {
        }

        public SettingsController(IIpFilterService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ApiLogData(ApiLogFilterGridModel filter)
        {
            var dbModel = _apiLogService.GetLogs();
            var data = dbModel
                .Where(x => (string.IsNullOrEmpty(filter.RequestUri.ToUpper()) || x.RequestUri.Contains(filter.RequestUri.ToUpper()))
                    && (string.IsNullOrEmpty(filter.AbsoluteUri) || x.AbsoluteUri.ToUpper().Contains(filter.AbsoluteUri.ToUpper()))
                    && (string.IsNullOrEmpty(filter.RequestMethod) || x.RequestMethod.ToUpper() == filter.RequestMethod.ToUpper())
                    && (string.IsNullOrEmpty(filter.RequestIpAddress) || x.RequestIpAddress.Contains(filter.RequestIpAddress))
                    && (!filter.ResponseStatusCode.HasValue || x.ResponseStatusCode == filter.ResponseStatusCode))
                .ToList();

            var dataResult = new
            {
                data = data
                    .OrderByDescending(x => x.ResponseTimestamp)
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
                itemsCount = data.Count()
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
    }
}
