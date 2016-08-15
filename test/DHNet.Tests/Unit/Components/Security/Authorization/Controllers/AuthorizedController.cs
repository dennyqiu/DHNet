using DHNet.Components.Security;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace DHNet.Tests.Unit.Components.Security
{
    [GlobalizedAuthorize]
    [ExcludeFromCodeCoverage]
    public class AuthorizedController : Controller
    {
        [HttpGet]
        public ViewResult Action()
        {
            return null;
        }

        [HttpPost]
        public ViewResult Action(Object obj)
        {
            return null;
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult AllowAnonymousAction()
        {
            return null;
        }

        [HttpGet]
        [AllowUnauthorized]
        public ViewResult AllowUnauthorizedAction()
        {
            return null;
        }
    }
}
