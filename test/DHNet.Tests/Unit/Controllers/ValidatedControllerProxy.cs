using DHNet.Controllers;
using DHNet.Services;
using DHNet.Validators;
using System.Web.Mvc;

namespace DHNet.Tests.Unit.Controllers
{
    public class ValidatedControllerProxy : ValidatedController<IValidator, IService>
    {
        protected ValidatedControllerProxy(IValidator validator, IService service)
            : base(validator, service)
        {
        }

        public void BaseOnActionExecuting(ActionExecutingContext filterContext)
        {
            OnActionExecuting(filterContext);
        }
    }
}
