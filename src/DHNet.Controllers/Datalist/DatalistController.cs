using Datalist;
using DHNet.Components.Datalists;
using DHNet.Components.Mvc;
using DHNet.Components.Security;
using DHNet.Data.Core;
using DHNet.Objects;
using System;
using System.Web.Mvc;
using System.Web.SessionState;

namespace DHNet.Controllers
{
    [AllowUnauthorized]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class DatalistController : BaseController
    {
        private IUnitOfWork UnitOfWork { get; set; }
        private Boolean Disposed { get; set; }

        public DatalistController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        [NonAction]
        public virtual JsonResult GetData(AbstractDatalist datalist, DatalistFilter filter)
        {
            datalist.CurrentFilter = filter;

            return Json(datalist.GetData(), JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public JsonResult Role(DatalistFilter filter)
        {
            return GetData(new Datalist<Role, RoleView>(UnitOfWork), filter);
        }

        protected override void Dispose(Boolean disposing)
        {
            if (Disposed) return;

            UnitOfWork.Dispose();
            Disposed = true;

            base.Dispose(disposing);
        }
    }
}
