using DHNet.Components.Security;
using System.Web.Mvc;

namespace DHNet.Tests.Unit.Components.Security
{
    public class GlobalizedAuthorizeAttributeProxy : GlobalizedAuthorizeAttribute
    {
        public void BaseHandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            HandleUnauthorizedRequest(filterContext);
        }
    }
}
