using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DHNet.Data.Core;
using DHNet.Objects;
using System.Data.Entity.Infrastructure;

namespace DHNet.Services
{
    public class TestTableService: BaseService, ITestTableService
    {
        public TestTableService (IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IQueryable<TestTableView> GetViews()
        {
            return UnitOfWork
               .Select<TestTable>()
               .To<TestTableView>();
          
        }

        public virtual void SeedTestTables(TestTableView view) 
        {
  
            IEnumerable<TestTable> testtable2 = GetData(); 
          
        }


        private IEnumerable<TestTable> GetData()
        {
            return UnitOfWork
                .Select<TestTable>()
                .ToArray()
                .Select(testtable => new TestTable
                {
                    Id = testtable.Id,
                    x = testtable.x,
                    y = testtable.y,
                    z = testtable.z
                });

        }

    }
}
