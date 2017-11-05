using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BS.Admin.Web.Filters
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        private RequiredAttribute _required = new RequiredAttribute();
        public string DependentProperty { get; set; }
        public object TargetValue { get; set; }

        public RequiredIfAttribute() 
        {
        }

        public RequiredIfAttribute(string dependentProperty, object targetValue)
        {
            this.DependentProperty = dependentProperty;
            this.TargetValue = targetValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(DependentProperty);
            if (property == null)
            {
                return new ValidationResult(string.Format("Unknown property: {0}", DependentProperty));
            }
            var otherValue = property.GetValue(validationContext.ObjectInstance, null);

            if (object.Equals(otherValue, TargetValue) && !_required.IsValid(value))
            {
                return new ValidationResult("Required");
            }

            return null;
        }
    }
}