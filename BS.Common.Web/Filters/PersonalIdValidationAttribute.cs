using BS.Common.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BS.Common.Web.Filters
{
    public class PersonalIdValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || value.ToString() == string.Empty)
                return null;

            var result = ValidationHelper.CheckPersonalIdNumber(value.ToString());
            if (!result)
            {
                return new ValidationResult("Bulstat is not valid");
            }

            return null;
        }
    }
}