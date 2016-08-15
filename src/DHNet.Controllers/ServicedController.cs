using DHNet.Services;
using System;
using System.Web.Mvc;

namespace DHNet.Controllers
{
    public abstract class ServicedController<TService> : BaseController
        where TService : IService
    {
        public TService Service { get; private set; }
        private Boolean Disposed { get; set; }

        protected ServicedController(TService service)
        {
            Service = service;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            Service.CurrentAccountId = CurrentAccountId;
        }

        protected override void Dispose(Boolean disposing)
        {
            if (Disposed) return;

            Service.Dispose();
            Disposed = true;

            base.Dispose(disposing);
        }
    }
}
