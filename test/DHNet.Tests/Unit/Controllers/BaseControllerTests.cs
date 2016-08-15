using DHNet.Components.Alerts;
using DHNet.Components.Mvc;
using DHNet.Components.Security;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;
using Xunit.Extensions;

namespace DHNet.Tests.Unit.Controllers
{
    public class BaseControllerTests : ControllerTests, IDisposable
    {
        private BaseControllerProxy controller;
        private String controllerName;
        private String areaName;

        public BaseControllerTests()
        {
            HttpContextBase httpContext = HttpContextFactory.CreateHttpContextBase();
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();

            controller = Substitute.ForPartsOf<BaseControllerProxy>();
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = httpContext;
            controller.ControllerContext.Controller = controller;
            controller.ControllerContext.RouteData =
                httpContext.Request.RequestContext.RouteData;
            controller.Url = Substitute.For<UrlHelper>();

            controllerName = controller.RouteData.Values["controller"] as String;
            areaName = controller.RouteData.Values["area"] as String;
        }
        public void Dispose()
        {
            GlobalizationManager.Provider = null;
            Authorization.Provider = null;
        }

        #region BaseController()

        [Fact]
        public void BaseController_SetsAuthorization()
        {
            IAuthorizationProvider actual = controller.AuthorizationProvider;
            IAuthorizationProvider expected = Authorization.Provider;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BaseController_CreatesEmptyAlerts()
        {
            Assert.Empty(controller.Alerts);
        }

        #endregion

        #region NotEmptyView(Object model)

        [Fact]
        public void NotEmptyView_NullModel_RedirectsToNotFound()
        {
            Object expected = RedirectToNotFound(controller);
            Object actual = controller.NotEmptyView(null);

            Assert.Same(expected, actual);
        }

        [Fact]
        public void NotEmptyView_ReturnsModelView()
        {
            Object expected = new Object();
            Object actual = (controller.NotEmptyView(expected) as ViewResult).Model;

            Assert.Same(expected, actual);
        }

        #endregion

        #region RedirectToLocal(String url)

        [Fact]
        public void RedirectToLocal_NotLocalUrl_RedirectsToDefault()
        {
            controller.Url.IsLocalUrl("www.test.com").Returns(false);

            Object expected = RedirectToDefault(controller);
            Object actual = controller.RedirectToLocal("www.test.com");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectToLocal_IsLocalUrl_RedirectsToLocal()
        {
            controller.Url.IsLocalUrl("/").Returns(true);

            String actual = (controller.RedirectToLocal("/") as RedirectResult).Url;
            String expected = "/";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region RedirectToDefault()

        [Fact]
        public void RedirectToDefault_Route()
        {
            RouteValueDictionary actual = controller.RedirectToDefault().RouteValues;

            Assert.Equal("Home", actual["controller"]);
            Assert.Equal("Index", actual["action"]);
            Assert.Equal("", actual["area"]);
            Assert.Equal(3, actual.Count);
        }

        #endregion

        #region RedirectToNotFound()

        [Fact]
        public void RedirectToNotFound_Route()
        {
            RouteValueDictionary actual = controller.RedirectToNotFound().RouteValues;

            Assert.Equal("NotFound", actual["action"]);
            Assert.Equal("Home", actual["controller"]);
            Assert.Equal("", actual["area"]);
            Assert.Equal(3, actual.Count);
        }

        #endregion

        #region RedirectToUnauthorized()

        [Fact]
        public void RedirectToUnauthorized_Route()
        {
            RouteValueDictionary actual = controller.RedirectToUnauthorized().RouteValues;

            Assert.Equal("Unauthorized", actual["action"]);
            Assert.Equal("Home", actual["controller"]);
            Assert.Equal("", actual["area"]);
        }

        #endregion

        #region RedirectIfAuthorized(String actionName)

        [Fact]
        public void RedirectIfAuthorized_Action_NotAuthorized_RedirectsToDefault()
        {
            controller.IsAuthorizedFor("Action", controllerName, areaName).Returns(false);

            Object expected = RedirectToDefault(controller);
            Object actual = controller.RedirectIfAuthorized("Action");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectIfAuthorized_Action_RedirectsToAction()
        {
            controller.IsAuthorizedFor("Action", controllerName, areaName).Returns(true);

            RouteValueDictionary expected = controller.BaseRedirectToAction("Action").RouteValues;
            RouteValueDictionary actual = controller.RedirectIfAuthorized("Action").RouteValues;

            Assert.Equal(expected["controller"], actual["controller"]);
            Assert.Equal(expected["language"], actual["language"]);
            Assert.Equal(expected["action"], actual["action"]);
            Assert.Equal(expected["area"], actual["area"]);
        }

        #endregion

        #region RedirectIfAuthorized(String actionName, Object routeValues)

        [Fact]
        public void RedirectIfAuthorized_Action_Route_NotAuthorized_RedirectsToDefault()
        {
            controller.IsAuthorizedFor("Action", controllerName, areaName).Returns(false);

            Object expected = RedirectToDefault(controller);
            Object actual = controller.RedirectIfAuthorized("Action", new { id = "Id" });

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectIfAuthorized_Action_Route_RedirectsToAction()
        {
            controller.IsAuthorizedFor("Action", controllerName, areaName).Returns(true);

            RouteValueDictionary expected = controller.BaseRedirectToAction("Action", new { id = "Id" }).RouteValues;
            RouteValueDictionary actual = controller.RedirectIfAuthorized("Action", new { id = "Id" }).RouteValues;

            Assert.Equal(expected["controller"], actual["controller"]);
            Assert.Equal(expected["language"], actual["language"]);
            Assert.Equal(expected["action"], actual["action"]);
            Assert.Equal(expected["area"], actual["area"]);
            Assert.Equal(expected["id"], actual["id"]);
        }

        #endregion

        #region RedirectIfAuthorized(String actionName, String controllerName)

        [Fact]
        public void RedirectIfAuthorized_Action_Controller_NotAuthorized_RedirectsToDefault()
        {
            controller.IsAuthorizedFor("Action", "Controller", areaName).Returns(false);

            Object expected = RedirectToDefault(controller);
            Object actual = controller.RedirectIfAuthorized("Action", "Controller");

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectIfAuthorized_Action_Controller_RedirectsToAction()
        {
            controller.IsAuthorizedFor("Action", "Controller", areaName).Returns(true);

            RouteValueDictionary expected = controller.BaseRedirectToAction("Action", "Controller").RouteValues;
            RouteValueDictionary actual = controller.RedirectIfAuthorized("Action", "Controller").RouteValues;

            Assert.Equal(expected["controller"], actual["controller"]);
            Assert.Equal(expected["language"], actual["language"]);
            Assert.Equal(expected["action"], actual["action"]);
            Assert.Equal(expected["area"], actual["area"]);
        }

        #endregion

        #region RedirectIfAuthorized(String actionName, String controllerName, Object routeValues)

        [Fact]
        public void RedirectIfAuthorized_Action_Controller_Route_NotAuthorized_RedirectsToDefault()
        {
            controller.IsAuthorizedFor("Action", "Controller", areaName).Returns(false);

            Object expected = RedirectToDefault(controller);
            Object actual = controller.RedirectIfAuthorized("Action", "Controller", new { id = "Id" });

            Assert.Same(expected, actual);
        }

        [Fact]
        public void RedirectIfAuthorized_Action_NullController_NullRoute_RedirectsToAction()
        {
            controller.IsAuthorizedFor("Action", controllerName, areaName).Returns(true);

            RouteValueDictionary expected = controller.BaseRedirectToAction("Action", null, null).RouteValues;
            RouteValueDictionary actual = controller.RedirectIfAuthorized("Action", null, null).RouteValues;

            Assert.Equal(expected["controller"], actual["controller"]);
            Assert.Equal(expected["language"], actual["language"]);
            Assert.Equal(expected["action"], actual["action"]);
            Assert.Equal(expected["area"], actual["area"]);
        }

        [Fact]
        public void RedirectIfAuthorized_Action_Controller_NullRoute_RedirectsToAction()
        {
            controller.IsAuthorizedFor("Action", "Controller", areaName).Returns(true);

            RouteValueDictionary expected = controller.BaseRedirectToAction("Action", "Controller", null).RouteValues;
            RouteValueDictionary actual = controller.RedirectIfAuthorized("Action", "Controller", null).RouteValues;

            Assert.Equal(expected["controller"], actual["controller"]);
            Assert.Equal(expected["language"], actual["language"]);
            Assert.Equal(expected["action"], actual["action"]);
            Assert.Equal(expected["area"], actual["area"]);
        }

        [Fact]
        public void RedirectIfAuthorized_Action_Controller_Route_RedirectsToAction()
        {
            controller.IsAuthorizedFor("Action", "Controller", "Area").Returns(true);

            RouteValueDictionary expected = controller.BaseRedirectToAction("Action", "Controller", new { area = "Area", id = "Id" }).RouteValues;
            RouteValueDictionary actual = controller.RedirectIfAuthorized("Action", "Controller", new { area = "Area", id = "Id" }).RouteValues;

            Assert.Equal(expected["controller"], actual["controller"]);
            Assert.Equal(expected["language"], actual["language"]);
            Assert.Equal(expected["action"], actual["action"]);
            Assert.Equal(expected["area"], actual["area"]);
            Assert.Equal(expected["id"], actual["id"]);
        }

        #endregion

        #region IsAuthorizedFor(String action, String controller, String area)

        [Fact]
        public void IsAuthorizedFor_NullAuthorization_ReturnsTrue()
        {
            Authorization.Provider = null;
            controller = Substitute.ForPartsOf<BaseControllerProxy>();

            Assert.Null(controller.AuthorizationProvider);
            Assert.True(controller.IsAuthorizedFor(null, null, null));
        }

        [Fact]
        public void IsAuthorizedFor_ReturnsAuthorizationResult()
        {
            Authorization.Provider.IsAuthorizedFor(controller.CurrentAccountId, "Area", "Controller", "Action").Returns(true);

            Assert.True(controller.IsAuthorizedFor("Action", "Controller", "Area"));
        }

        #endregion

        #region BeginExecuteCore(AsyncCallback callback, Object state)

        [Theory]
        [InlineData("", 0)]
        [InlineData("1", 1)]
        [InlineData(null, 0)]
        public void BeginExecuteCore_SetsCurrentAccountId(String identityName, Int32 accountId)
        {
            controller.ControllerContext.HttpContext.User.Identity.Name.Returns(identityName);
            GlobalizationManager.Provider = Substitute.For<IGlobalizationProvider>();

            controller.BaseBeginExecuteCore(asyncResult => { }, null);

            Int32? actual = controller.CurrentAccountId;
            Int32? expected = accountId;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BeginExecuteCore_SetsCurrentLanguage()
        {
            GlobalizationManager.Provider = Substitute.For<IGlobalizationProvider>();
            GlobalizationManager.Provider["lt"].Returns(new Language());
            controller.RouteData.Values["language"] = "lt";

            controller.BaseBeginExecuteCore(asyncResult => { }, null);

            Language actual = GlobalizationManager.Provider.CurrentLanguage;
            Language expected = GlobalizationManager.Provider["lt"];

            Assert.Equal(expected, actual);
        }

        #endregion

        #region OnAuthorization(AuthorizationContext filterContext)

        [Fact]
        public void OnAuthorization_NotAuthenticated_SetsNullResult()
        {
            ActionDescriptor descriptor = Substitute.ForPartsOf<ActionDescriptor>();
            AuthorizationContext filterContext = new AuthorizationContext(controller.ControllerContext, descriptor);
            controller.ControllerContext.HttpContext.User.Identity.IsAuthenticated.Returns(false);

            controller.BaseOnAuthorization(filterContext);

            Assert.Null(filterContext.Result);
        }

        [Fact]
        public void OnAuthorization_NotAuthorized_RedirectsToUnauthorized()
        {
            controller.When(sub => sub.RedirectToUnauthorized()).DoNotCallBase();
            controller.ControllerContext.HttpContext.User.Identity.IsAuthenticated.Returns(true);
            controller.RedirectToUnauthorized().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            AuthorizationContext context = new AuthorizationContext(controller.ControllerContext, Substitute.ForPartsOf<ActionDescriptor>());

            controller.BaseOnAuthorization(context);

            ActionResult expected = controller.RedirectToUnauthorized();
            ActionResult actual = context.Result;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OnAuthorization_IsAuthorized_SetsNullResult()
        {
            AuthorizationContext context = new AuthorizationContext(controller.ControllerContext, Substitute.ForPartsOf<ActionDescriptor>());
            Authorization.Provider.IsAuthorizedFor(controller.CurrentAccountId, "Area", "Controller", "Action").Returns(true);
            controller.ControllerContext.HttpContext.User.Identity.IsAuthenticated.Returns(true);
            context.RouteData.Values["controller"] = "Controller";
            context.RouteData.Values["action"] = "Action";
            context.RouteData.Values["area"] = "Area";

            controller.BaseOnAuthorization(context);

            Assert.Null(context.Result);
        }

        #endregion

        #region OnActionExecuted(ActionExecutedContext filterContext)

        [Fact]
        public void OnActionExecuted_NullTempDataAlerts_SetsTempDataAlerts()
        {
            controller.TempData["Alerts"] = null;
            controller.BaseOnActionExecuted(new ActionExecutedContext());

            Object actual = controller.TempData["Alerts"];
            Object expected = controller.Alerts;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void OnActionExecuted_MergesTempDataAlerts()
        {
            HttpContextBase context = controller.HttpContext;

            BaseControllerProxy mergedController = new BaseControllerProxy();
            mergedController.ControllerContext = new ControllerContext();
            mergedController.ControllerContext.HttpContext = context;
            mergedController.TempData = controller.TempData;
            mergedController.Alerts.AddError("Test1");

            IEnumerable<Alert> controllerAlerts = controller.Alerts;
            IEnumerable<Alert> mergedAlerts = mergedController.Alerts;

            controller.Alerts.AddError("Test2");
            controller.BaseOnActionExecuted(new ActionExecutedContext());
            mergedController.BaseOnActionExecuted(new ActionExecutedContext());

            IEnumerable<Alert> actual = controller.TempData["Alerts"] as AlertsContainer;
            IEnumerable<Alert> expected = controllerAlerts.Union(mergedAlerts);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
