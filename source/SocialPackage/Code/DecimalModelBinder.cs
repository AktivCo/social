using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialPackage.Code
{
    public class DecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object actualValue = null;
            var modelState = new ModelState();
            try
            {
                var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                if (string.IsNullOrEmpty(valueResult.AttemptedValue))
                {
                    return 0m;
                }
                modelState.Value = valueResult;

                try
                {
                    actualValue = Convert.ToDecimal(
                        valueResult.AttemptedValue.Replace(",", "."),
                        CultureInfo.InvariantCulture
                    );
                }
                catch (FormatException e)
                {
                    modelState.Errors.Add(e);
                }
            }
            catch (Exception e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}