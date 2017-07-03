using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS.Common;

namespace BS.Common.Models
{
    public class LicenserInfoModel
    {
        public string Name { get; set; }

        public bool IsCompany { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ConactPerson { get; set; }

        public override string ToString()
        {
            return string.Format(
@"Име: {0}, Физическо лице: {1}, Телефон: {2}, Email: {3}, Лице за контакт: {4}", Name, (!IsCompany).ToBgString(), Phone, Email, ConactPerson);
        }
    }
}
