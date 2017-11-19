using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Admin.Web.Models
{
    public class UserIndexModel
    {
        public UserLicenseSortedCollection Real { get; set; }
        public UserLicenseSortedCollection Demo { get; set; }
    }
}