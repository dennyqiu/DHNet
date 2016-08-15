using DHNet.Services;
using DHNet.Validators;
using System;
using System.Web.Mvc;

namespace DHNet.Controllers
{
    public abstract class ValidatedController<TValidator, TService> : ServicedController<TService>
        where TValidator : IValidator
        where TService : IService
    {
        public TValidator Validator { get; private set; }
        private Boolean Disposed { get; set; }

        protected ValidatedController(TValidator validator, TService service)
            : base(service)
        {
            Validator = validator;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            Validator.CurrentAccountId = Service.CurrentAccountId;
            Validator.ModelState = ModelState;
            Validator.Alerts = Alerts;
        }

        protected override void Dispose(Boolean disposing)
        {
            if (Disposed) return;

            Validator.Dispose();
            Disposed = true;

            base.Dispose(disposing);
        }
    }
}
