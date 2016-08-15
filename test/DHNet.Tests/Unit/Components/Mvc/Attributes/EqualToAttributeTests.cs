using DHNet.Components.Mvc;
using DHNet.Resources;
using DHNet.Resources.Form;
using DHNet.Tests.Objects;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace DHNet.Tests.Unit.Components.Mvc
{
    public class EqualToAttributeTests
    {
        private EqualToAttribute attribute;

        public EqualToAttributeTests()
        {
            attribute = new EqualToAttribute("Total");
        }

        #region EqualToAttribute(String otherPropertyName)

        [Fact]
        public void EqualToAttribute_SetsOtherPropertyName()
        {
            String actual = new EqualToAttribute("Other").OtherPropertyName;
            String expected = "Other";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FormatErrorMessage(String name)

        [Fact]
        public void FormatErrorMessage_ForProperty()
        {
            attribute.OtherPropertyDisplayName = "Other";

            String expected = String.Format(Validations.EqualTo, "Sum", attribute.OtherPropertyDisplayName);
            String actual = attribute.FormatErrorMessage("Sum");

            Assert.Equal(expected, actual);
        }

        #endregion

        #region IsValid(Object value, ValidationContext validationContext)

        [Fact]
        public void IsValid_EqualValue()
        {
            AttributesModel model = new AttributesModel();
            ValidationContext context = new ValidationContext(model);

            Assert.Null(attribute.GetValidationResult(model.Sum, context));
        }

        [Fact]
        public void IsValid_SetsOtherPropertyDisplayName()
        {
            AttributesModel model = new AttributesModel { Total = 10 };
            ValidationContext context = new ValidationContext(model);

            attribute.GetValidationResult(model.Sum, context);

            String expected = ResourceProvider.GetPropertyTitle(context.ObjectType, attribute.OtherPropertyName);
            String actual = attribute.OtherPropertyDisplayName;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsValid_NotEqualValue_ReturnsValidationResult()
        {
            AttributesModel model = new AttributesModel { Total = 10 };
            ValidationContext context = new ValidationContext(model);

            ValidationResult expected = new ValidationResult(attribute.FormatErrorMessage(context.DisplayName));
            ValidationResult actual = attribute.GetValidationResult(model.Sum, context);

            Assert.Equal(expected.ErrorMessage, actual.ErrorMessage);
        }

        #endregion
    }
}
