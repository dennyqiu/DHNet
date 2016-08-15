using System;
using System.Security.Principal;

namespace DHNet.Components.Security
{
    public static class IPrincipalExtensions
    {
        public static Int32? Id(this IPrincipal principal)
        {
            String id = principal.Identity.Name;
            if (String.IsNullOrEmpty(id))
                return null;

            return Int32.Parse(id);
        }
    }
}
