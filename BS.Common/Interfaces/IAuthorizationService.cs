using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Common.Interfaces
{
    public interface IAuthorizationService
    {
        bool Authorize(string key);
    }
}
