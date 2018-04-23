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
            CompanyId  = model.CompanyId.Trim();
            EGN  = model.EGN.Trim();
            PostCode  = model.PostCode;
            RegistrationAddress  = model.RegistrationAddress;
            PostAddress  = model.PostAddress;
            MOL  = model.MOL;
            AccountingPerson = model.AccountingPerson; 
            VATRegistration  = model.VATRegistration;
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
                CompanyId = (this.CompanyId ?? string.Empty).ToString().Trim(),
                EGN = (this.EGN ?? string.Empty).ToString().Trim(),
                PostCode = this.PostCode,
                RegistrationAddress = this.RegistrationAddress,
                PostAddress = this.PostAddress,
                MOL = this.MOL,
                AccountingPerson = this.AccountingPerson,
                VATRegistration = this.VATRegistration,
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

        /// <summary>
        /// https://github.com/mirovit/eik-validator/blob/master/src/EIKValidator/EIKValidator.php
        /// https://github.com/Dr4g0/MyProjects/blob/master/TestEGN_BUSLTAT/TestIDNumber.cs
        /// </summary>
        [Display(Name = "Булстат")]
        [RequiredIf("IsCompany", true)]
        //[RangeIf("IsCompany", true, 9999999999999, 999999999999999, ErrorMessage = "Въведете само цифри.Полето трябва да е между 13 и 15 символа.")]
        [CompanyIdValidation(ErrorMessage = "Невалиден Булстат номер")]
        public string CompanyId { get; set; }

        [Display(Name = "ЕГН")]
        //[RangeIf("IsCompany", false, 1000000000, 9999999999, ErrorMessage = "Въведете само цифри.Полето трябва да е 10 символа.")]
        [RequiredIf("IsCompany", false)]
        [PersonalIdValidation(ErrorMessage = "Невалиден ЕГН номер")]
        public string EGN { get; set; }

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
        public bool VATRegistration { get; set; }
    }
}
