using DHNet.Controllers;
using System;
using System.Web.Mvc;

namespace DHNet.Tests.Unit.Controllers
{
    public class BaseControllerProxy : BaseController
    {
        public RedirectToRouteResult BaseRedirectToAction(String actionName)
        {
            return RedirectToAction(actionName);
        }
        public RedirectToRouteResult BaseRedirectToAction(String actionName, Object routeValues)
        {
            return RedirectToAction(actionName, routeValues);
        }
        public RedirectToRouteResult BaseRedirectToAction(String actionName, String controllerName)
        {
            return RedirectToAction(actionName, controllerName);
        }
        public RedirectToRouteResult BaseRedirectToAction(String actionName, String controllerName, Object routeValues)
        {
            return RedirectToAction(actionName, controllerName, routeValues);
        }

        public IAsyncResult BaseBeginExecuteCore(AsyncCallback callback, Object state)
        {
            return BeginExecuteCore(callback, state);
        }
        public void BaseOnAuthorization(AuthorizationContext filterContext)
        {
            OnAuthorization(filterContext);
        }
        public void BaseOnActionExecuted(ActionExecutedContext filterContext)
        {
            OnActionExecuted(filterContext);
        }
    }
}
