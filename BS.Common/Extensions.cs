using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BS.Common
{
    public static class Extensions
    {
        public static string ToBgString(this bool value)
        {
            return value ? "Да" : "Не";
        }

        public static string Description(this Enum value) 
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi == null)
                return string.Empty;

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
