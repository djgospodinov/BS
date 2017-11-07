using BS.Admin.Web.Filters;
using BS.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BS.Admin.Web.Models
{
    public class CreateLicenseOwnerModel
    {
        public CreateLicenseOwnerModel() 
        {
        }

        public CreateLicenseOwnerModel(LicenserInfoModel model)
        {
            Id = model.Id;
            Name  = model.Name;
            IsDemo  = model.IsDemo;
            IsCompany  = model.IsCompany;
            Phone  = model.Phone;
            Email  = model.Email;
            CompanyId  = Convert.ToInt32(model.CompanyId);
            EGN  = Convert.ToInt32(model.EGN);
            PostCode  = model.PostCode;
            RegistrationAddress  = model.RegistrationAddress;
            PostAddress  = model.PostAddress;
            MOL  = model.MOL;
            AccountingPerson = model.AccountingPerson; 
            DDSRegistration  = model.DDSRegistration;
            ContactPerson = model.ContactPerson;
        }

        public LicenserInfoModel ToDbModel()
        {
            return new LicenserInfoModel()
            {
                Id = this.Id,
                Name = this.Name,
                IsDemo = this.IsDemo,
                IsCompany = this.IsCompany,
                Phone = this.Phone,
                Email = this.Email,
                CompanyId = this.CompanyId.ToString(),
                EGN = this.EGN.ToString(),
                PostCode = this.PostCode,
                RegistrationAddress = this.RegistrationAddress,
                PostAddress = this.PostAddress,
                MOL = this.MOL,
                AccountingPerson = this.AccountingPerson,
                DDSRegistration = this.DDSRegistration,
                ContactPerson = this.ContactPerson
            };
        }

        [Display(Name = "Идентификатор")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Име")]
        public string Name { get; set; }

        [Display(Name = "Демо")]
        public bool IsDemo { get; set; }

        [Display(Name = "Фирма")]
        public bool IsCompany { get; set; }

        [Display(Name = "Телефон")]
        [Required]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [Required]
        [EmailAddressAttribute(ErrorMessage = "Невалиден имейл")]
        public string Email { get; set; }

        [Display(Name = "Лице за контакт")]
        [Required]
        public string ContactPerson { get; set; }

        [Display(Name = "Булстат")]
        [RequiredIf("IsCompany", true)]
        [Range(0, int.MaxValue, ErrorMessage = "Въведете само цифри.")]
        public int? CompanyId { get; set; }

        [Display(Name = "ЕГН")]
        [Range(0, int.MaxValue, ErrorMessage = "Въведете само цифри.")]
        [RequiredIf("IsCompany", false)]
        public int? EGN { get; set; }

        [Display(Name = "Пощенски код")]
        public int PostCode { get; set; }

        [Display(Name = "Адрес на регистрация")]
        public string RegistrationAddress { get; set; }

        [Display(Name = "Пощенски адрес")]
        public string PostAddress { get; set; }

        [Display(Name = "МОЛ")]
        public string MOL { get; set; }

        [Display(Name = "Счетоводител")]
        public string AccountingPerson { get; set; }

        [Display(Name = "С регистрация по ДДС")]
        public bool DDSRegistration { get; set; }
    }
}
