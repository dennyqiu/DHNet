using DHNet.Resources.Form;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DHNet.Components.Mvc
{
    public class StringLengthAdapter : StringLengthAttributeAdapter
    {
        public StringLengthAdapter(ModelMetadata metadata, ControllerContext context, StringLengthAttribute attribute)
            : base(metadata, context, attribute)
        {
            if (Attribute.MinimumLength == 0)
                Attribute.ErrorMessage = Validations.StringLength;
            else
                Attribute.ErrorMessage = Validations.StringLengthRange;
        }
    }
}