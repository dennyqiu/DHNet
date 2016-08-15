using DHNet.Resources.Form;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DHNet.Components.Mvc
{
    public class NumberValidator : ModelValidator
    {
        public NumberValidator(ModelMetadata metadata, ControllerContext context)
            : base(metadata, context)
        {
        }

        public override IEnumerable<ModelValidationResult> Validate(Object container)
        {
            return new ModelValidationResult[0];
        }
        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            yield return new ModelClientValidationRule
            {
                ValidationType = "number",
                ErrorMessage = String.Format(Validations.Numeric, Metadata.GetDisplayName())
            };
        }
    }
}
