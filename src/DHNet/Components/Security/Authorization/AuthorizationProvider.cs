using DHNet.Components.Mvc;
using DHNet.Data.Core;
using DHNet.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace DHNet.Components.Security
{
    public class AuthorizationProvider : IAuthorizationProvider
    {
        private IEnumerable<Type> Controllers { get; set; }
        private Dictionary<String, String> Required { get; set; }
        private Dictionary<Int32, HashSet<String>> Permissions { get; set; }

        public AuthorizationProvider(Assembly controllersAssembly)
        {
            Controllers = GetValidControllers(controllersAssembly);
            Permissions = new Dictionary<Int32, HashSet<String>>();
            Required = new Dictionary<String, String>();

            foreach (Type type in Controllers)
            {
                foreach (MethodInfo method in GetValidMethods(type))
                {
                    String permission = (GetArea(type) + "/" + GetController(type) + "/" + GetAction(method)).ToLower();
                    String requiredPermission = GetRequiredPermission(type, method);

                    if (requiredPermission != null && !Required.ContainsKey(permission))
                        Required[permission] = requiredPermission;
                }
            }
        }

        public Boolean IsAuthorizedFor(Int32? accountId, String area, String controller, String action)
        {
            String permission = (area + "/" + controller + "/" + action).ToLower();
            if (!Required.ContainsKey(permission))
                return true;

            if (!Permissions.ContainsKey(accountId ?? 0))
                return false;

            return Permissions[accountId.Value].Contains(Required[permission]);
        }

        public void Refresh()
        {
            using (IUnitOfWork unitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>())
            {
                Permissions = unitOfWork
                    .Select<Account>()
                    .Where(account =>
                        !account.IsLocked &&
                        account.RoleId != null)
                    .Select(account => new
                    {
                        Id = account.Id,
                        Permissions = account
                            .Role
                            .Permissions
                            .Select(role => role.Permission)
                            .Select(permission => (permission.Area + "/" + permission.Controller + "/" + permission.Action).ToLower())
                    })
                    .ToDictionary(
                        account => account.Id,
                        account => new HashSet<String>(account.Permissions));
            }
        }

        private IEnumerable<MethodInfo> GetValidMethods(Type controller)
        {
            return controller
                    .GetMethods(
                        BindingFlags.DeclaredOnly |
                        BindingFlags.InvokeMethod |
                        BindingFlags.Instance |
                        BindingFlags.Public)
                    .Where(method =>
                        !method.IsDefined(typeof(NonActionAttribute)) &&
                        !method.IsSpecialName)
                    .OrderByDescending(method =>
                        method.IsDefined(typeof(HttpGetAttribute), false));
        }
        private IEnumerable<Type> GetValidControllers(Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(type =>
                    type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) &&
                    typeof(Controller).IsAssignableFrom(type) &&
                    !type.IsAbstract &&
                    type.IsPublic);
        }

        private String GetRequiredPermission(Type type, MethodInfo method)
        {
            AuthorizeAsAttribute authorizeAs = GetAuthorizeAs(method);
            String controller = GetController(type);
            String action = GetAction(method);
            String area = GetArea(type);

            if (authorizeAs != null)
            {
                type = GetControllerType(authorizeAs.Area ?? area, authorizeAs.Controller ?? controller);
                method = GetMethod(type, authorizeAs.Action);

                return GetRequiredPermission(type, method);
            }

            if (AllowsUnauthorized(type, method)) return null;

            return (area + "/" + controller + "/" + action).ToLower();
        }
        private String GetAction(MethodInfo method)
        {
            if (method.IsDefined(typeof(ActionNameAttribute), false))
                return method.GetCustomAttribute<ActionNameAttribute>(false).Name;

            return method.Name;
        }
        private String GetController(Type type)
        {
            return type.Name.Substring(0, type.Name.Length - 10);
        }
        private String GetArea(Type type)
        {
            if (!type.IsDefined(typeof(AreaAttribute), false))
                return null;

            return type.GetCustomAttribute<AreaAttribute>(false).Name;
        }

        private Boolean AllowsUnauthorized(Type controller, MethodInfo method)
        {
            if (method.IsDefined(typeof(AuthorizeAttribute), false)) return false;
            if (method.IsDefined(typeof(AllowAnonymousAttribute), false)) return true;
            if (method.IsDefined(typeof(AllowUnauthorizedAttribute), false)) return true;

            while (controller != typeof(Controller))
            {
                if (controller.IsDefined(typeof(AuthorizeAttribute), false)) return false;
                if (controller.IsDefined(typeof(AllowAnonymousAttribute), false)) return true;
                if (controller.IsDefined(typeof(AllowUnauthorizedAttribute), false)) return true;

                controller = controller.BaseType;
            }

            return true;
        }
        private Type GetControllerType(String area, String controller)
        {
            String typeName = controller + "Controller";
            IEnumerable<Type> controllers = Controllers
                .Where(type => type.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase));

            if (String.IsNullOrEmpty(area))
                controllers = controllers.Where(type =>
                    !type.IsDefined(typeof(AreaAttribute), false));
            else
                controllers = controllers.Where(type =>
                    type.IsDefined(typeof(AreaAttribute), false) &&
                    String.Equals(type.GetCustomAttribute<AreaAttribute>(false).Name, area, StringComparison.OrdinalIgnoreCase));

            return controllers.Single();
        }
        private AuthorizeAsAttribute GetAuthorizeAs(MemberInfo action)
        {
            if (!action.IsDefined(typeof(AuthorizeAsAttribute), false))
                return null;

            return action.GetCustomAttribute<AuthorizeAsAttribute>(false);
        }
        private MethodInfo GetMethod(Type controller, String action)
        {
            return controller
                .GetMethods()
                .Where(method =>
                    (
                        !method.IsDefined(typeof(ActionNameAttribute), false) &&
                        method.Name.ToLowerInvariant() == action.ToLowerInvariant()
                    )
                    ||
                    (
                        method.IsDefined(typeof(ActionNameAttribute), false) &&
                        method.GetCustomAttribute<ActionNameAttribute>(false).Name.ToLowerInvariant() == action.ToLowerInvariant()
                    ))
                .OrderByDescending(method =>
                    method.IsDefined(typeof(HttpGetAttribute), false))
                .First();
        }
    }
}
