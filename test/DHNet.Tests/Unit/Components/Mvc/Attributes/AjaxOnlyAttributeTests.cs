using DHNet.Components.Mvc;
using NSubstitute;
using System;
using System.Web.Mvc;
using Xunit;
using Xunit.Extensions;

namespace DHNet.Tests.Unit.Components.Mvc
{
    public class AjaxOnlyAttributeTests
    {
        #region IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)

        [Theory]
        [InlineData("", false)]
        [InlineData("XMLHttpRequest", true)]
        public void IsValidForRequest_Ajax(String headerValue, Boolean expected)
        {
            ControllerContext context = new ControllerContext();
            context.HttpContext = HttpContextFactory.CreateHttpContextBase();
            context.HttpContext.Request["X-Requested-With"].Returns(headerValue);

            Boolean actual = new AjaxOnlyAttribute().IsValidForRequest(context, null);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
