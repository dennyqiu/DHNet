using DHNet.Components.Mvc;
using DHNet.Tests.Objects;
using System;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace DHNet.Tests.Unit.Components.Mvc
{
    public class ModelStateDictionaryExtensionsTests
    {
        private ModelStateDictionary modelState;

        public ModelStateDictionaryExtensionsTests()
        {
            modelState = new ModelStateDictionary();
        }

        #region AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, Object>> expression, Exception exception)

        [Fact]
        public void AddModelError_ExceptionKey()
        {
            modelState.AddModelError<AllTypesView>(model => model.Child.StringField, new Exception());

            String actual = modelState.Single().Key;
            String expected = "Child.StringField";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddModelError_ExceptionValue()
        {
            Exception exception = new Exception();
            modelState.AddModelError<AllTypesView>(model => model.Child.StringField, exception);

            Object actual = modelState.Single().Value.Errors.Single().Exception;
            Object expected = exception;

            Assert.Same(expected, actual);
        }

        #endregion

        #region AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, Object>> expression, String errorMessage)

        [Fact]
        public void AddModelError_Key()
        {
            modelState.AddModelError<AllTypesView>(model => model.Child.NullableByteField, "Test error");

            String expected = "Child.NullableByteField";
            String actual = modelState.Single().Key;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddModelError_Message()
        {
            modelState.AddModelError<AllTypesView>(model => model.Child.NullableByteField, "Test error");

            String actual = modelState.Single().Value.Errors.Single().ErrorMessage;
            String expected = "Test error";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region AddModelError<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, Object>> expression, String format, Object[] args)

        [Fact]
        public void AddModelError_FormattedKey()
        {
            modelState.AddModelError<AllTypesView>(model => model.Int32Field, "Test {0}", "error");

            String actual = modelState.Single().Key;
            String expected = "Int32Field";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddModelError_FormattedMessage()
        {
            modelState.AddModelError<AllTypesView>(model => model.Int32Field, "Test {0}", "error");

            String actual = modelState.Single().Value.Errors.Single().ErrorMessage;
            String expected = "Test error";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
