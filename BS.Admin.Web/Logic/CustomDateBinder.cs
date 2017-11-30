﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BS.Admin.Web
{
    public class CustomDateBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext", "controllerContext is null.");

            if (bindingContext == null)
                throw new ArgumentNullException("bindingContext", "bindingContext is null.");

            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == null)
                throw new ArgumentNullException(bindingContext.ModelName);
            
            CultureInfo cultureInf = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            cultureInf.DateTimeFormat.ShortDatePattern = Const.ShortDatePattern;

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value);
            try
            {
                var date = value.ConvertTo(typeof(DateTime), cultureInf);
                return date;
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex);
                return null;
            }
        }
    }
}