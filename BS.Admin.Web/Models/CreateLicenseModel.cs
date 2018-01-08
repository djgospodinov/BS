using BS.Common.Interfaces;
using BS.Common.Models;
using BS.LicenseServer.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BS.Admin.Web.Models
{
    public class CreateLicenseModel
    {
        public CreateLicenseModel() 
        {
            
        }

        public CreateLicenseModel(BS.Common.LicenseModel model)
            :this()
        {
            Id = model.Id;
            ValidTo = model.ValidTo;
            SubscribedTo = model.SubscribedTo;
            IsDemo = model.IsDemo;
            UserId = model.User != null ? model.User.Id : 0;
            Type = (int)model.Type;
            Enabled = model.Enabled;
            IsActivated = model.IsActivated;
            Created = model.Created;
            WorkstationsCount = model.WorkstationsCount ?? 1;

            if (model.LicenseModules.Contains(LicenseModulesEnum.Accounting))
            {
                this.Accounting = true;
            }

            if (model.LicenseModules.Contains(LicenseModulesEnum.Store))
            {
                this.Warehouse = true;
            }

            if (model.LicenseModules.Contains(LicenseModulesEnum.Payroll))
            {
                this.Salary = true;
            }

            if (model.LicenseModules.Contains(LicenseModulesEnum.Schedule))
            {
                this.Schedules = true;
            }
        }

        public CreateLicenseModel(CreateLicenseModel model)
            :this()
        {
            Id = model.Id;
            ValidTo = model.ValidTo;
            SubscribedTo = model.SubscribedTo;
            IsDemo = model.IsDemo;
            UserId = model.UserId;
            Type = model.Type;
            Enabled = model.Enabled;
            IsActivated = model.IsActivated;
            Created = model.Created;
            WorkstationsCount = model.WorkstationsCount;

            Accounting = model.Accounting;
            //Production = model.Production;
            Warehouse = model.Warehouse;
            //TradingSystem = model.TradingSystem;
            Salary = model.Salary;
            Schedules = model.Schedules;
        }

        public BS.Common.LicenseModel ToDbModel(IUserService userService) 
        {
            var result = new Common.LicenseModel()
            {
                Id = this.Id,
                ValidTo = this.ValidTo,
                SubscribedTo = this.SubscribedTo,
                IsDemo = this.IsDemo,
                User = userService.Get(this.UserId.Value),
                Type = (LicenseTypeEnum)this.Type,
                Enabled = this.Enabled,
                IsActivated = this.IsActivated,
                Created = this.Created,
                WorkstationsCount = this.WorkstationsCount
            };

            result.LicenseModules = new List<LicenseModulesEnum>();
            if (this.Accounting) 
            {
                result.LicenseModules.Add(LicenseModulesEnum.Accounting);
            }

            if (this.Warehouse)
            {
                result.LicenseModules.Add(LicenseModulesEnum.Store);
            }

            if (this.Salary)
            {
                result.LicenseModules.Add(LicenseModulesEnum.Payroll);
            }

            if (this.Schedules)
            {
                result.LicenseModules.Add(LicenseModulesEnum.Schedule);
            }

            return result;
        }

        public UpdateLicenseModel ToUpdateDbModel(IUserService userService)
        {
            var result = new UpdateLicenseModel()
            {
                ValidTo = this.ValidTo,
                SubscribedTo = this.SubscribedTo,
                IsDemo = this.IsDemo,
                UserId = this.UserId.Value,
                Type = (LicenseTypeEnum)this.Type,
                Enabled = this.Enabled,
                IsActivated = this.IsActivated,
                ComputerCount = this.WorkstationsCount
            };

            result.Modules = new List<LicenseModulesEnum>();
            if (this.Accounting)
            {
                result.Modules.Add(LicenseModulesEnum.Accounting);
            }

            if (this.Warehouse)
            {
                result.Modules.Add(LicenseModulesEnum.Store);
            }

            if (this.Salary)
            {
                result.Modules.Add(LicenseModulesEnum.Payroll);
            }

            if (this.Schedules)
            {
                result.Modules.Add(LicenseModulesEnum.Schedule);
            }

            return result;
        }

        public Guid Id { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        public DateTime? SubscribedTo { get; set; }

        [Required]
        public bool IsDemo { get; set; }

        [Required]
        public int? UserId { get; set; }

        [Required]
        public int Type { get; set; }

        public bool Enabled { get; set; }

        public bool IsActivated { get; set; }

        public DateTime Created { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int WorkstationsCount { get; set; }

        #region Modules
        public bool Accounting { get; set; }

        //public bool Production { get; set; }

        public bool Warehouse { get; set; }

        //public bool TradingSystem { get; set; }

        public bool Salary { get; set; }

        public bool Schedules { get; set; }
        #endregion
    }
}