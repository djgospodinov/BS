using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Common.Models
{
    public class LicenseLogChangeItem
    {
        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }

    public class LicenseLogModel
    {
        public int Id { get; set; }
        public Guid LicenseId { get; set; }
        public DateTime Date { get; set; }
        public bool IsDemo { get; set; }
        public long ChangedBy { get; set; }
        public Dictionary<string, LicenseLogChangeItem> Changes { get; set; }
        public string Old { get; set; }
        public string New { get; set; }
    }
}
