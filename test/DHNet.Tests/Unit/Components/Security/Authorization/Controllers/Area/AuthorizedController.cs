using DHNet.Components.Mvc;
using DHNet.Components.Security;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace DHNet.Tests.Unit.Components.Security.Area
{
    [Area("Area")]
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
        public ViewResult AuthorizedGetAction()
        {
            return null;
        }

        [HttpPost]
        [AllowAnonymous]
        public ViewResult AuthorizedGetAction(Object obj)
        {
            return null;
        }

        [HttpPost]
        public ViewResult AuthorizedPostAction()
        {
            return null;
        }

        [HttpGet]
        [ActionName("AuthorizedNamedGetAction")]
        public ViewResult GetActionWithName()
        {
            return null;
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("AuthorizedNamedGetAction")]
        public ViewResult GetActionWithName(Object obj)
        {
            return null;
        }

        [HttpPost]
        [ActionName("AuthorizedNamedPostAction")]
        public ViewResult PostActionWithName()
        {
            return null;
        }

        [HttpGet]
        [AuthorizeAs("Action")]
        public ViewResult AuthorizedAsAction()
        {
            return null;
        }

        [HttpGet]
        [AuthorizeAs("InheritanceAction", Controller = "InheritedAuthorized", Area = "")]
        public ViewResult AuthorizedAsOtherAction()
        {
            return null;
        }
    }
}
