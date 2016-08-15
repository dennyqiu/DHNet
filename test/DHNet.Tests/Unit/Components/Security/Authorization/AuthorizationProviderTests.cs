using DHNet.Components.Security;
using DHNet.Data.Core;
using DHNet.Objects;
using DHNet.Tests.Data;
using NSubstitute;
using System;
using System.Reflection;
using System.Web.Mvc;
using Xunit;
using Xunit.Extensions;

namespace DHNet.Tests.Unit.Components.Security
{
    public class AuthorizationProviderTests : IDisposable
    {
        private AuthorizationProvider provider;

        public AuthorizationProviderTests()
        {
            provider = new AuthorizationProvider(Assembly.GetExecutingAssembly());
            using (TestingContext context = new TestingContext()) context.DropData();
        }
        public void Dispose()
        {
            DependencyResolver.SetResolver(Substitute.For<IDependencyResolver>());
        }

        #region IsAuthorizedFor(Int32? accountId, String area, String controller, String action)

        [Fact]
        public void IsAuthorizedFor_AuthorizesControllerByIgnoringCase()
        {
            Account account = CreateAccountWithPermissionFor("Area", "Authorized", "Action");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "AUTHORIZED", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeControllerByIgnoringCase()
        {
            Account account = CreateAccountWithPermissionFor("Test", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "AUTHORIZED", "Action"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void IsAuthorizedFor_AuthorizesControllerWithoutArea(String area)
        {
            Account account = CreateAccountWithPermissionFor(null, "Authorized", "Action");

            Assert.True(provider.IsAuthorizedFor(account.Id, area, "Authorized", "Action"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void IsAuthorizedFor_DoesNotAuthorizeControllerWithoutArea(String area)
        {
            Account account = CreateAccountWithPermissionFor(null, "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, area, "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesControllerWithArea()
        {
            Account account = CreateAccountWithPermissionFor("Area", "Authorized", "Action");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeControllerWithArea()
        {
            Account account = CreateAccountWithPermissionFor("Test", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesGetAction()
        {
            Account account = CreateAccountWithPermissionFor("Area", "Authorized", "AuthorizedGetAction");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedGetAction"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeGetAction()
        {
            Account account = CreateAccountWithPermissionFor("Test", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedGetAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesNamedGetAction()
        {
            Account account = CreateAccountWithPermissionFor("Area", "Authorized", "AuthorizedNamedGetAction");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedNamedGetAction"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeNamedGetAction()
        {
            Account account = CreateAccountWithPermissionFor("Test", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedNamedGetAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesNotExistingAction()
        {
            Assert.True(provider.IsAuthorizedFor(null, null, "Authorized", "Test"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesNonGetAction()
        {
            Account account = CreateAccountWithPermissionFor("Area", "Authorized", "AuthorizedPostAction");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedPostAction"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeNonGetAction()
        {
            Account account = CreateAccountWithPermissionFor("Test", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedPostAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesNamedNonGetAction()
        {
            Account account = CreateAccountWithPermissionFor("Area", "Authorized", "AuthorizedNamedPostAction");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedNamedPostAction"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeNamedNonGetAction()
        {
            Account account = CreateAccountWithPermissionFor("Area", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedNamedPostAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesActionAsAction()
        {
            Account account = CreateAccountWithPermissionFor("Area", "Authorized", "Action");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedAsAction"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeActionAsAction()
        {
            Account account = CreateAccountWithPermissionFor(null, "Authorized", "Action");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedAsAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesActionAsOtherAction()
        {
            Account account = CreateAccountWithPermissionFor(null, "InheritedAuthorized", "InheritanceAction");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedAsOtherAction"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeActionAsOtherAction()
        {
            Account account = CreateAccountWithPermissionFor("Area", "Authorized", "AuthorizedAsOtherAction");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "AuthorizedAsOtherAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesEmptyAreaAsNull()
        {
            Account account = CreateAccountWithPermissionFor(null, "Authorized", "Action");

            Assert.True(provider.IsAuthorizedFor(account.Id, "", "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeEmptyAreaAsNull()
        {
            Account account = CreateAccountWithPermissionFor(null, "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "", "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesAuthorizedAction()
        {
            Account account = CreateAccountWithPermissionFor(null, "AllowAnonymous", "AuthorizedAction");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "AllowAnonymous", "AuthorizedAction"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeAuthorizedAction()
        {
            Account account = CreateAccountWithPermissionFor(null, "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, null, "AllowAnonymous", "AuthorizedAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousAction()
        {
            Account account = CreateAccountWithPermissionFor(null, "Test", "Test");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AllowAnonymousAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedAction()
        {
            Account account = CreateAccountWithPermissionFor(null, "Test", "Test");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "Authorized", "AllowUnauthorizedAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesAuthorizedController()
        {
            Account account = CreateAccountWithPermissionFor("Area", "Authorized", "Action");

            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeAuthorizedController()
        {
            Account account = CreateAccountWithPermissionFor("Test", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesAllowAnonymousController()
        {
            Account account = CreateAccountWithPermissionFor(null, "Test", "Test");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "AllowAnonymous", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesAllowUnauthorizedController()
        {
            Account account = CreateAccountWithPermissionFor(null, "Test", "Test");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "AllowUnauthorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesInheritedAuthorizedController()
        {
            Account account = CreateAccountWithPermissionFor(null, "InheritedAuthorized", "InheritanceAction");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "InheritedAuthorized", "InheritanceAction"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeInheritedAuthorizedController()
        {
            Account account = CreateAccountWithPermissionFor(null, "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, null, "InheritedAuthorized", "InheritanceAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesInheritedAllowAnonymousController()
        {
            Account account = CreateAccountWithPermissionFor(null, "Test", "Test");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "InheritedAllowAnonymous", "InheritanceAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesInheritedAllowUnauthorizedController()
        {
            Account account = CreateAccountWithPermissionFor(null, "Test", "Test");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "InheritedAllowUnauthorized", "InheritanceAction"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesNotAttributedController()
        {
            Account account = CreateAccountWithPermissionFor(null, "Test", "Test");

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "NotAttributed", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeNotExistingAccount()
        {
            Account account = CreateAccountWithPermissionFor("Area", "Authorized", "Action");

            Assert.False(provider.IsAuthorizedFor(0, "Area", "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeLockedAccount()
        {
            Account account = CreateAccountWithPermissionFor("Area", "Authorized", "Action", isLocked: true);

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeNullAccount()
        {
            Account account = CreateAccountWithPermissionFor(null, "Authorized", "Action");

            Assert.False(provider.IsAuthorizedFor(null, null, "Authorized", "Action"));
        }

        [Fact]
        public void IsAuthorizedFor_AuthorizesByIgnoringCase()
        {
            Account account = CreateAccountWithPermissionFor("Area", "Authorized", "Action");

            Assert.True(provider.IsAuthorizedFor(account.Id, "area", "authorized", "action"));
        }

        [Fact]
        public void IsAuthorizedFor_DoesNotAuthorizeByIgnoringCase()
        {
            Account account = CreateAccountWithPermissionFor("Test", "Test", "Test");

            Assert.False(provider.IsAuthorizedFor(account.Id, "area", "authorized", "action"));
        }

        [Fact]
        public void IsAuthorizedFor_CachesAccountPermissions()
        {
            Account account = CreateAccountWithPermissionFor(null, "Authorized", "Action");
            using (TestingContext context = new TestingContext()) context.DropData();

            Assert.True(provider.IsAuthorizedFor(account.Id, null, "Authorized", "Action"));
        }

        #endregion

        #region Refresh()

        [Fact]
        public void Refresh_Permissions()
        {
            Account account = CreateAccountWithPermissionFor("Area", "Authorized", "Action");
            Assert.True(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "Action"));

            using (TestingContext context = new TestingContext()) context.DropData();
            SetUpDependencyResolver();

            provider.Refresh();

            Assert.False(provider.IsAuthorizedFor(account.Id, "Area", "Authorized", "Action"));
        }

        #endregion

        #region Test helpers

        private Account CreateAccountWithPermissionFor(String area, String controller, String action, Boolean isLocked = false)
        {
            using (TestingContext context = new TestingContext())
            {
                RolePermission rolePermission = ObjectFactory.CreateRolePermission();
                Account account = ObjectFactory.CreateAccount();
                account.RoleId = rolePermission.RoleId;
                account.IsLocked = isLocked;
                account.Role = null;

                rolePermission.Permission.Controller = controller;
                rolePermission.Permission.Action = action;
                rolePermission.Permission.Area = area;

                context.Set<RolePermission>().Add(rolePermission);
                context.Set<Account>().Add(account);
                context.SaveChanges();

                SetUpDependencyResolver();
                provider.Refresh();

                return account;
            }
        }

        private void SetUpDependencyResolver()
        {
            IDependencyResolver resolver = Substitute.For<IDependencyResolver>();
            resolver.GetService<IUnitOfWork>().Returns(new UnitOfWork(new TestingContext()));

            DependencyResolver.SetResolver(resolver);
        }

        #endregion
    }
}
