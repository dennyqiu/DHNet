using DHNet.Components.Security;
using System.Diagnostics.CodeAnalysis;

namespace DHNet.Tests.Unit.Components.Security
{
    [AllowUnauthorized]
    [ExcludeFromCodeCoverage]
    public class AllowUnauthorizedController : AuthorizedController
    {
    }
}
