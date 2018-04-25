using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BS.Common;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using BS.Common.Web.Filters;
using BS.Common.Web;

namespace BS.Common.Models
{
    [DataContract]
    public class LicenserInfoModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string Name { get; set; }

        [DataMember]
        public bool IsDemo { get; set; }

        [DataMember]
        public bool IsCompany { get; set; }

        [DataMember]
        [Required]
        public string Phone { get; set; }

        [DataMember]
        [Required]
        [EmailAddressAttribute(ErrorMessage = "Невалиден имейл")]
        public string Email { get; set; }

        [Required]
        public string UniqueId
        {
            get
            {
                return IsCompany ? CompanyId : EGN;
            }
        }

        [DataMember]
        [RequiredIf("IsCompany", true)]
        [CompanyIdValidation(ErrorMessage = "Невалиден Булстат номер")]
        public string CompanyId { get; set; }

        [DataMember]
        //[Required]
        public string EGN { get; set; }

        [DataMember]
        public int PostCode { get; set; }

        [DataMember]
        public string RegistrationAddress { get; set; }

        [DataMember]
        public string PostAddress { get; set; }

        [DataMember]
        public string MOL { get; set; }

        [DataMember]
        public string AccountingPerson { get; set; }

        [DataMember]
        public bool VATRegistration { get; set; }
        
        [DataMember]
        public string ContactPerson { get; set; }

        public override string ToString()
        {
            var result = string.Format(@"Име: {0}, Физическо лице: {1}, Телефон: {2}, Email: {3}, Лице за контакт: {4}, Булстат: {5}", Name, (!IsCompany).ToBgString(), Phone, Email, ContactPerson, CompanyId);
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
