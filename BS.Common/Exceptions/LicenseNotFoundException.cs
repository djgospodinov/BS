using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Common.Exceptions
{
    public class LicenseNotFoundException : KeyNotFoundException
    {
        public LicenseNotFoundException(string message)
            : base(message) 
        {
        }
    }
}
