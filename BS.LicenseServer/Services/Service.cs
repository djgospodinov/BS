﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.LicenseServer.Services
{
    public abstract class Service
    {
        public Service(int? userId)
        {
            UserId = userId;
        }

        public int? UserId { get; set; }
    }
}
