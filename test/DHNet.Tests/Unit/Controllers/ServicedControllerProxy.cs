using DHNet.Controllers;
using DHNet.Services;
using System.Web.Mvc;

namespace DHNet.Tests.Unit.Controllers
{
    public class ServicedControllerProxy : ServicedController<IService>
    {
        public ServicedControllerProxy(IService service) : base(service)
        {
        }

        public void BaseOnActionExecuting(ActionExecutingContext filterContext)
        {
            OnActionExecuting(filterContext);
        }
    }
}
