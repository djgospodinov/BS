using BS.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Common.Interfaces
{
    public interface IIpFilterService
    {
        IpAddressElement Get(int id);

        List<IpAddressElement> GetAll();

        bool UseIpFiltering();

        bool Add(IpAddressElement ipAddressElement);
        
        bool Edit(int id, IpAddressElement ipAddressElement);

        bool Delete(int id);

        void SetUseIpFiltering(bool useRestriction);
    }
}
