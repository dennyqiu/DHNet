using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DHNet.Objects;
using System.Data.Entity.Infrastructure;


namespace DHNet.Services
{
    public interface ITestTableService:IService
    { 
        void SeedTestTables(TestTableView view); 

        IQueryable<TestTableView> GetViews();
        //void TestTable(IEnumerable<DbEntityEntry<BaseModel>> entries);
        //IEnumerable<TestTable> GetData(); 
    }
}
