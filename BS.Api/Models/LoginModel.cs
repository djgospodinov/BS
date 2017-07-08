using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS.Api.Models
{
    public class LoginModel
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}