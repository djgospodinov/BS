using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BS.LicenseServer.Cache
{
    public abstract class BaseCache
    {
        public BaseCache(TimeSpan interval)
        {
            var timer = new Timer();
            timer.Interval = interval.TotalMilliseconds;
            timer.Elapsed += (s, e) =>
            {
                Initiliaze();
            };
            timer.Start();

            Initiliaze();
        }

        protected abstract void Initiliaze();
    }
}
