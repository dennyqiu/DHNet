using DHNet.Components.Alerts;
using DHNet.Data.Core;
using System;
using System.Web.Mvc;

namespace DHNet.Validators
{
    public abstract class BaseValidator : IValidator
    {
        protected IUnitOfWork UnitOfWork { get; private set; }
        public ModelStateDictionary ModelState { get; set; }
        public Int32 CurrentAccountId { get; set; }
        public AlertsContainer Alerts { get; set; }
        private Boolean Disposed { get; set; }

        protected BaseValidator(IUnitOfWork unitOfWork)
        {
            ModelState = new ModelStateDictionary();
            Alerts = new AlertsContainer();
            UnitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            if (Disposed) return;

            UnitOfWork.Dispose();

            Disposed = true;
        }
    }
}
