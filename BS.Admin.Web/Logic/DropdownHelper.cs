using BS.Common.Interfaces;
using BS.Common.Models;
using BS.LicenseServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BS.Common;

namespace BS.Admin.Web
{
    public class DropdownHelper
    {
        protected static readonly List<LicenseTypeEnum> _types = new List<LicenseTypeEnum>() 
            {
                LicenseTypeEnum.PerComputer,
                LicenseTypeEnum.PerUser,
                LicenseTypeEnum.PerServer
            };

        private static readonly IUserService _service = new UserService();

        public static SelectList LicenseTypes() 
        {
            var result = new SelectList(_types.Select(x => new { Id = (int)x, Name = x.Description() }).ToList(), "Id", "Name");

            return result;
        }

        public static IEnumerable<SelectListItem> LicenseUsers()
        {
            var result = new SelectList(_service.GetAll().Select(x => new { Id = x.Id, Name = string.Format(string.Format("{0}({1})", x.Name, x.UniqueId)) }), "Id", "Name");

            return result;
        }

        private static List<object> AllRoles = new List<object>()
                {
                    new { Id = 1, Name = "Administrator" },
                    new { Id = 2, Name = "SuperUser" },
                    new { Id = 3, Name = "NormalUser" },
                };

        public static SelectList Roles()
        {
            var result = new SelectList(AllRoles, "Id", "Name");

            return result;
        }
    }
}