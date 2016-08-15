using DHNet.Components.Mvc;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DHNet.Tests.Unit.Components.Mvc
{
    public class DisplayNameMetadataProviderProxy : DisplayNameMetadataProvider
    {
        public ModelMetadata BaseCreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<Object> modelAccessor, Type modelType, String propertyName)
        {
            return CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
        }
    }
}
