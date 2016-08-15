using DHNet.Components.Alerts;
using DHNet.Components.Mvc;
using DHNet.Components.Security;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace DHNet.Controllers
{
    [GlobalizedAuthorize]
    public abstract class BaseController : Controller
    {
        public IAuthorizationProvider AuthorizationProvider { get; protected set; }
        public virtual Int32 CurrentAccountId { get; protected set; }
        public AlertsContainer Alerts { get; protected set; }

        protected BaseController()
        {
            AuthorizationProvider = Authorization.Provider;
            Alerts = new AlertsContainer();
        }

        public virtual ActionResult NotEmptyView(Object model)
        {
            if (model == null)
                return RedirectToNotFound();

            return View(model);
        }
        public virtual ActionResult RedirectToLocal(String url)
        {
            if (!Url.IsLocalUrl(url))
                return RedirectToDefault();

            return Redirect(url);
        }
        public virtual RedirectToRouteResult RedirectToDefault()
        {
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        public virtual RedirectToRouteResult RedirectToNotFound()
        {
            return RedirectToAction("NotFound", "Home", new { area = "" });
        }
        public virtual RedirectToRouteResult RedirectToUnauthorized()
        {
            return RedirectToAction("Unauthorized", "Home", new { area = "" });
        }
        public virtual RedirectToRouteResult RedirectIfAuthorized(String actionName)
        {
            return RedirectIfAuthorized(actionName, null, null);
        }
        public virtual RedirectToRouteResult RedirectIfAuthorized(String actionName, Object routeValues)
        {
            return RedirectIfAuthorized(actionName, null, routeValues);
        }
        public virtual RedirectToRouteResult RedirectIfAuthorized(String actionName, String controllerName)
        {
            return RedirectIfAuthorized(actionName, controllerName, null);
        }
        public virtual RedirectToRouteResult RedirectIfAuthorized(String actionName, String controllerName, Object routeValues)
        {
            RouteValueDictionary values = new RouteValueDictionary(routeValues);
            String area = (values["area"] ?? RouteData.Values["area"]) as String;
            String controller = (controllerName ?? values["controller"] ?? RouteData.Values["controller"]) as String;

            if (!IsAuthorizedFor(actionName, controller, area))
                return RedirectToDefault();

            return RedirectToAction(actionName, controllerName, values);
        }

        public virtual Boolean IsAuthorizedFor(String action, String controller, String area)
        {
            if (AuthorizationProvider == null) return true;

            return AuthorizationProvider.IsAuthorizedFor(CurrentAccountId, area, controller, action);
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, Object state)
        {
            CurrentAccountId = User.Id() ?? 0;

            String abbreviation = RouteData.Values["language"].ToString();
            GlobalizationManager.Provider.CurrentLanguage = GlobalizationManager.Provider[abbreviation];

            return base.BeginExecuteCore(callback, state);
        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!User.Identity.IsAuthenticated) return;

            String area = filterContext.RouteData.Values["area"] as String;
            String action = filterContext.RouteData.Values["action"] as String;
            String controller = filterContext.RouteData.Values["controller"] as String;

            if (!IsAuthorizedFor(action, controller, area))
                filterContext.Result = RedirectToUnauthorized();
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            AlertsContainer current = TempData["Alerts"] as AlertsContainer;
            if (current == null)
                TempData["Alerts"] = Alerts;
            else
                current.Merge(Alerts);
        }
    }
}
