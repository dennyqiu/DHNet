using DHNet.Controllers;
using NSubstitute;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace DHNet.Tests.Unit.Controllers
{
    public abstract class ControllerTests
    {
        protected void ReturnCurrentAccountId(BaseController controller, Int32 id)
        {
            controller.When(sub => { Int32 get = sub.CurrentAccountId; }).DoNotCallBase();
            controller.CurrentAccountId.Returns(id);
        }

        protected RedirectToRouteResult NotEmptyView(BaseController controller, Object model)
        {
            RedirectToRouteResult result = new RedirectToRouteResult(new RouteValueDictionary());
            controller.When(sub => sub.NotEmptyView(model)).DoNotCallBase();
            controller.NotEmptyView(model).Returns(result);

            return result;
        }

        protected RedirectToRouteResult RedirectToDefault(BaseController controller)
        {
            RedirectToRouteResult result = new RedirectToRouteResult(new RouteValueDictionary());
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(result);

            return result;
        }
        protected RedirectToRouteResult RedirectToNotFound(BaseController controller)
        {
            RedirectToRouteResult result = new RedirectToRouteResult(new RouteValueDictionary());
            controller.When(sub => sub.RedirectToNotFound()).DoNotCallBase();
            controller.RedirectToNotFound().Returns(result);

            return result;
        }
        protected RedirectToRouteResult RedirectIfAuthorized(BaseController controller, String actionName)
        {
            RedirectToRouteResult result = new RedirectToRouteResult(new RouteValueDictionary());
            controller.When(sub => sub.RedirectIfAuthorized(actionName)).DoNotCallBase();
            controller.RedirectIfAuthorized(actionName).Returns(result);

            return result;
        }
        protected RedirectToRouteResult RedirectIfAuthorized(BaseController controller, String actionName, String controllerName)
        {
            RedirectToRouteResult result = new RedirectToRouteResult(new RouteValueDictionary());
            controller.When(sub => sub.RedirectIfAuthorized(actionName, controllerName)).DoNotCallBase();
            controller.RedirectIfAuthorized(actionName, controllerName).Returns(result);

            return result;
        }
    }
}
