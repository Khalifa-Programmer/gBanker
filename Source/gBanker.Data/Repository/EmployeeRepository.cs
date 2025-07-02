using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using gBanker.Data.CodeFirstMigration;

namespace gBanker.Data.Repository
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
     
        IEnumerable<EmployeeDetail> getEmployeeList(int? officeID, string EmpID);
    }
    public class EmployeeRepository : RepositoryBaseCodeFirst<gBanker.Data.CodeFirstMigration.Db.Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<EmployeeDetail> getEmployeeList(int? officeID, string EmpID)
        {

           
            var officeIdParameter = new SqlParameter("@officeID", officeID);
            var filColvalue = new SqlParameter("@EmpID", EmpID);
         
            return DataContext.Database.SqlQuery<EmployeeDetail>("getEmployee @officeID,@EmpID", officeIdParameter,   filColvalue);

        }

        //public IEnumerable<getEmployeeInfo_Result> GetEmployee()
        //{
        //    //return DataContext.getEmployeeInfo();
        //    return null;
        //}


    }
}
