using LightInject;
using LightInject.Mvc;
using DHNet.Components.Logging;
using DHNet.Components.Mvc;
using DHNet.Components.Security;
using DHNet.Controllers;
using DHNet.Web.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DHNet.Web
{
    public class MvcApplication : HttpApplication
    {
        public void Application_Start()
        {
            RegisterSecureResponseConfiguration();
            RegisterCurrentDependencyResolver();
            RegisterGlobalizationProvider();
            RegisterModelMetadataProvider();
            RegisterDataTypeValidator();
            RegisterSiteMapProvider();
            RegisterAuthorization();
            RegisterModelBinders();
            RegisterViewEngine();
            RegisterAdapters();
            RegisterBundles();
            RegisterAreas();
            RegisterRoute();
        }
        public void Application_Error()
        {
            ILogger logger = DependencyResolver.Current.GetService<ILogger>();
            Exception exception = Server.GetLastError();
            logger.Log(exception);

            if (Context.IsCustomErrorEnabled)
            {
                RouteValueDictionary route = new RouteValueDictionary(Request.RequestContext.RouteData.Values);
                HttpException httpException = exception as HttpException;
                UrlHelper url = new UrlHelper(Request.RequestContext);
                Server.ClearError();

                route["controller"] = "Home";
                route["action"] = "Error";
                route["area"] = "";

                if (httpException != null && httpException.GetHttpCode() == 404)
                    route["action"] = "NotFound";

                Response.TrySkipIisCustomErrors = true;
                Response.Redirect(url.RouteUrl(route));
            }
        }

        public virtual void RegisterSecureResponseConfiguration()
        {
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
            MvcHandler.DisableMvcResponseHeader = true;
        }
        public virtual void RegisterCurrentDependencyResolver()
        {
            MainContainer container = new MainContainer();
            container.RegisterControllers(typeof(BaseController).Assembly);
            container.RegisterServices();
            container.EnableMvc();

            DependencyResolver.SetResolver(new LightInjectMvcDependencyResolver(container));
        }
        public virtual void RegisterGlobalizationProvider()
        {
            GlobalizationManager.Provider = DependencyResolver.Current.GetService<IGlobalizationProvider>();
        }
        public virtual void RegisterModelMetadataProvider()
        {
            ModelMetadataProviders.Current = new DisplayNameMetadataProvider();
        }
        public virtual void RegisterDataTypeValidator()
        {
            ModelValidatorProviders.Providers.Remove(ModelValidatorProviders.Providers.SingleOrDefault(x => x is ClientDataTypeModelValidatorProvider));
            ModelValidatorProviders.Providers.Add(new DataTypeValidatorProvider());
        }
        public virtual void RegisterSiteMapProvider()
        {
            MvcSiteMap.Provider = DependencyResolver.Current.GetService<IMvcSiteMapProvider>();
        }
        public virtual void RegisterAuthorization()
        {
            Authorization.Provider = DependencyResolver.Current.GetService<IAuthorizationProvider>();
            Authorization.Provider.Refresh();
        }
        public virtual void RegisterModelBinders()
        {
            ModelBinders.Binders.Add(typeof(String), new TrimmingModelBinder());
        }
        public virtual void RegisterViewEngine()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ViewEngine());
        }
        public virtual void RegisterAdapters()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RangeAttribute), typeof(RangeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(DigitsAttribute), typeof(DigitsAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EqualToAttribute), typeof(EqualToAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(IntegerAttribute), typeof(IntegerAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredAttribute), typeof(RequiredAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MinValueAttribute), typeof(MinValueAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MaxValueAttribute), typeof(MaxValueAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MinLengthAttribute), typeof(MinLengthAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(GreaterThanAttribute), typeof(GreaterThanAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EmailAddressAttribute), typeof(EmailAddressAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(StringLengthAttribute), typeof(StringLengthAdapter));
        }
        public virtual void RegisterBundles()
        {
            DependencyResolver.Current.GetService<IBundleConfig>().RegisterBundles(BundleTable.Bundles);
        }
        public virtual void RegisterAreas()
        {
            AreaRegistration.RegisterAllAreas();
        }
        public virtual void RegisterRoute()
        {
            RouteTable.Routes.LowercaseUrls = true;
            DependencyResolver.Current.GetService<IRouteConfig>().RegisterRoutes(RouteTable.Routes);
        }
    }
}
