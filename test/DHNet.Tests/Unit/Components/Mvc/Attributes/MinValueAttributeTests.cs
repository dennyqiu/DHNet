using DHNet.Components.Mvc;
using DHNet.Resources.Form;
using System;
using Xunit;
using Xunit.Extensions;

namespace DHNet.Tests.Unit.Components.Mvc
{
    public class MinValueAttributeTests
    {
        private MinValueAttribute attribute;

        public MinValueAttributeTests()
        {
            attribute = new MinValueAttribute(12.56);
        }

        #region MinValueAttribute(Int32 minimum)

        [Fact]
        public void MinValueAttribute_ForInteger()
        {
            Decimal actual = new MinValueAttribute(10).Minimum;
            Decimal expected = 10M;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region MinValueAttribute(Double minimum)

        [Fact]
        public void MinValueAttribute_ForDouble()
        {
            Decimal actual = new MinValueAttribute(12.56).Minimum;
            Decimal expected = 12.56M;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region FormatErrorMessage(String name)

        [Fact]
        public void FormatErrorMessage_ForInteger()
        {
            attribute = new MinValueAttribute(10);

            String expected = String.Format(Validations.MinValue, "Sum", attribute.Minimum);
            String actual = attribute.FormatErrorMessage("Sum");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FormatErrorMessage_ForDouble()
        {
            attribute = new MinValueAttribute(12.56);

            String expected = String.Format(Validations.MinValue, "Sum", attribute.Minimum);
            String actual = attribute.FormatErrorMessage("Sum");

            Assert.Equal(expected, actual);
        }

        #endregion

        #region IsValid(Object value)

        [Fact]
        public void IsValid_Null()
        {
            Assert.True(attribute.IsValid(null));
        }

        [Theory]
        [InlineData(12.56)]
        [InlineData("12.561")]
        public void IsValid_GreaterOrEqualValue(Object value)
        {
            Assert.True(attribute.IsValid(value));
        }

        [Fact]
        public void IsValid_LowerValue_ReturnsFalse()
        {
            Assert.False(attribute.IsValid(12.559));
        }

        [Fact]
        public void IsValid_NotDecimalValueIsNotValid()
        {
            Assert.False(attribute.IsValid("12.56M"));
        }

        #endregion
    }
}
