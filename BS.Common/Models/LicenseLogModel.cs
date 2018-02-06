using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name ="Id")]
        public int Id { get; set; }
        [Display(Name = "Лиценз Id")]
        public Guid LicenseId { get; set; }
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
        [Display(Name = "Промяна от демо")]
        public bool IsDemo { get; set; }
        [Display(Name = "Променено от")]
        public string ChangedByName { get; set; }
        public long ChangedBy { get; set; }
        [Display(Name = "Промени")]
        public Dictionary<string, LicenseLogChangeItem> Changes { get; set; }
        [Display(Name = "Стар")]
        public string Old { get; set; }
        [Display(Name = "Нов")]
        public string New { get; set; }
    }
}
