using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace DHNet.Tests.Unit.Components.Security
{
    [ExcludeFromCodeCoverage]
    public class NotAttributedController : Controller
    {
        [HttpGet]
        public ViewResult Action()
        {
            return null;
        }
    }
}
