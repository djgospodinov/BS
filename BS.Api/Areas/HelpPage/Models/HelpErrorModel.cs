using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Api.Areas.HelpPage.Models
{
    public class HelpErrorModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}