using DHNet.Data.Core;
using System;

namespace DHNet.Services
{
    public abstract class BaseService : IService
    {
        protected IUnitOfWork UnitOfWork { get; private set; }
        public Int32 CurrentAccountId { get; set; }
        private Boolean Disposed { get; set; }

        protected BaseService(IUnitOfWork unitOfWork)
        {
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
