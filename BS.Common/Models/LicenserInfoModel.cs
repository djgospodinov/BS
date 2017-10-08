using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS.Common;

namespace BS.Common.Models
{
    public class LicenserInfoModelBase
    {
        public string Name { get; set; }

        public virtual bool IsCompany { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string ConactPerson { get; set; }

        public string CompanyId { get; set; }

        public string EGN { get; set; }

        public override string ToString()
        {
            return string.Format(
@"Име: {0}, Физическо лице: {1}, Телефон: {2}, Email: {3}, Лице за контакт: {4}, Булстат: {5}", Name, (!IsCompany).ToBgString(), Phone, Email, ConactPerson);
        }
    }

    public class DemoLicenserInfoModel : LicenserInfoModelBase
    {
        
    }

    public class RealLicenserInfoModel : LicenserInfoModelBase
    {
        public int PostCode { get; set; }

        public string RegistrationAddress { get; set; }

        public string PostAddress { get; set; }

        public string MOL { get; set; }

        public string ContactPerson { get; set; }

        public string AccountingPerson { get; set; }

        public bool DDSRegistration { get; set; }

        public override string ToString()
        {
            var result = base.ToString();
            if (result.Length > 0)
            {
                result = result + ' ' + string.Format("Булстат: {0}", CompanyId);
            }
            else 
            {
                result = result + string.Format("Булстат: {0}", CompanyId);
            }

             return result;
        }
    }
}
