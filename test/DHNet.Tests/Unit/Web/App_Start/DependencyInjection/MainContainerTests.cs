using DHNet.Components.Logging;
using DHNet.Components.Mail;
using DHNet.Components.Mvc;
using DHNet.Components.Security;
using DHNet.Controllers;
using DHNet.Data.Core;
using DHNet.Data.Logging;
using DHNet.Services;
using DHNet.Validators;
using DHNet.Web;
using DHNet.Web.DependencyInjection;
using System;
using System.Data.Entity;
using System.Web.Mvc;
using Xunit;
using Xunit.Extensions;

namespace DHNet.Tests.Unit.Web.DependencyInjection
{
    public class MainContainerTests
    {
        private static MainContainer container;

        static MainContainerTests()
        {
            container = new MainContainer();
            container.RegisterServices();
        }

        #region RegisterServices()

        [Theory]
        [InlineData(typeof(DbContext), typeof(Context))]
        [InlineData(typeof(IUnitOfWork), typeof(UnitOfWork))]

        [InlineData(typeof(ILogger), typeof(Logger))]
        [InlineData(typeof(IAuditLogger), typeof(AuditLogger))]

        [InlineData(typeof(IHasher), typeof(BCrypter))]
        [InlineData(typeof(IMailClient), typeof(SmtpMailClient))]

        [InlineData(typeof(IRouteConfig), typeof(RouteConfig))]
        [InlineData(typeof(IBundleConfig), typeof(BundleConfig))]

        [InlineData(typeof(IMvcSiteMapParser), typeof(MvcSiteMapParser))]

        [InlineData(typeof(IRoleService), typeof(RoleService))]
        [InlineData(typeof(IAccountService), typeof(AccountService))]

        [InlineData(typeof(IRoleValidator), typeof(RoleValidator))]
        [InlineData(typeof(IAccountValidator), typeof(AccountValidator))]
        public void RegisterServices_Transient(Type abstraction, Type expectedType)
        {
            Object expected = container.GetInstance(abstraction);
            Object actual = container.GetInstance(abstraction);

            Assert.IsType(expectedType, actual);
            Assert.NotSame(expected, actual);
        }

        [Theory]
        [InlineData(typeof(IAuthorizationProvider), typeof(AuthorizationProvider))]
        public void RegisterServices_Singleton(Type abstraction, Type expectedType)
        {
            Object expected = container.GetInstance(abstraction);
            Object actual = container.GetInstance(abstraction);

            Assert.IsType(expectedType, actual);
            Assert.Same(expected, actual);
        }

        #endregion
    }
}
