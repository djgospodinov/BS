using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BS.Common.Web
{
    public class RangeIfAttribute : RangeAttribute
    {
        public string DependentProperty { get; private set; }
        public object TargetValue { get; private set; }

        public RangeIfAttribute(string dependentProperty, object targetValue, int minimum, int maximum)
            : base(minimum, maximum)
        {
            DependentProperty = dependentProperty;
            TargetValue = targetValue;
        }

        public RangeIfAttribute(string dependentProperty, object targetValue, long minimum, long maximum)
            : base(minimum, maximum)
        {
            DependentProperty = dependentProperty;
            TargetValue = targetValue;
        }

        public RangeIfAttribute(string dependentProperty, object targetValue, double minimum, double maximum)
            : base(minimum, maximum)
        {
            DependentProperty = dependentProperty;
            TargetValue = targetValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(DependentProperty);
            if (property == null)
            {
                return new ValidationResult(string.Format("Unknown property: {0}", DependentProperty));
            }
            var otherValue = property.GetValue(validationContext.ObjectInstance, null);

            if (object.Equals(otherValue, TargetValue) && !IsValid(value))
            {
                return new ValidationResult("Range");
            }

            return null;
        }

        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return false;
            }

            var length = value.ToString().Length;
            var minLength = base.Minimum.ToString().Length;
            var maxLength = base.Maximum.ToString().Length;
            if (length > maxLength || length < minLength)
            {
                return false;
            }

            return true;
        }
    }
}