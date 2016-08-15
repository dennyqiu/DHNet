using DHNet.Components.Mvc;
using DHNet.Resources;
using DHNet.Tests.Objects;
using System;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace DHNet.Tests.Unit.Components.Mvc
{
    public class EqualToAdapterTests
    {
        #region GetClientValidationRules()

        [Fact]
        public void GetClientValidationRules_SetsOtherPropertyDisplayName()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(AdaptersModel), "EqualTo");
            EqualToAttribute attribute = new EqualToAttribute("StringLength");
            attribute.OtherPropertyDisplayName = null;

            new EqualToAdapter(metadata, new ControllerContext(), attribute).GetClientValidationRules();

            String expected = ResourceProvider.GetPropertyTitle(typeof(AdaptersModel), "EqualTo");
            String actual = attribute.OtherPropertyDisplayName;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetClientValidationRules_ReturnsEqualToValidationRule()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(AdaptersModel), "EqualTo");
            EqualToAdapter adapter = new EqualToAdapter(metadata, new ControllerContext(), new EqualToAttribute("StringLength"));

            String expectedMessage = new EqualToAttribute("StringLength").FormatErrorMessage(metadata.GetDisplayName());
            ModelClientValidationRule actual = adapter.GetClientValidationRules().Single();

            Assert.Equal("*.StringLength", actual.ValidationParameters["other"]);
            Assert.Equal(expectedMessage, actual.ErrorMessage);
            Assert.Equal("equalto", actual.ValidationType);
            Assert.Single(actual.ValidationParameters);
        }

        #endregion
    }
}
