using DHNet.Resources.Form;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DHNet.Components.Mvc
{
    public class MinLengthAdapter : MinLengthAttributeAdapter
    {
        public MinLengthAdapter(ModelMetadata metadata, ControllerContext context, MinLengthAttribute attribute)
            : base(metadata, context, attribute)
        {
            Attribute.ErrorMessage = Validations.MinLength;
        }
    }
}
