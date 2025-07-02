using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IEmployeeOfficeMappingRepository : IRepository<EmployeeOfficeMapping>
    {
        IEnumerable<EmployeeOfficeMapping> GetEmployeeOfficeMappings(string employeeCode);
        void CreateEmployeeOfficeMapping(int OrgID, string employeeCode, List<EmployeeOfficeMapping> mappings);

    }
    public class EmployeeOfficeMappingRepository : RepositoryBaseCodeFirst<EmployeeOfficeMapping>, IEmployeeOfficeMappingRepository
    {
        public EmployeeOfficeMappingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<EmployeeOfficeMapping> GetEmployeeOfficeMappings(string employeeCode)
        {
            var mappings = DataContext.EmployeeOfficeMappings.Where(w => w.Employee.EmployeeCode == employeeCode && w.IsActive == true && w.Employee.IsActive == true && w.Employee.EmployeeStatus==1).OrderBy(w=>w.Office.OfficeCode);
            return mappings;
        }

        public void CreateEmployeeOfficeMapping(int OrgID, string employeeCode, List<EmployeeOfficeMapping> mappings)
        {
            var employee = DataContext.Employees.Where(w => w.EmployeeCode == employeeCode && w.EmployeeStatus==1).FirstOrDefault();
            if (employee != null)
            {
                foreach (var map in mappings)
                {
                    var existingMapping = DataContext.EmployeeOfficeMappings.Where(e => e.EmployeeID == employee.EmployeeID && e.OfficeID == map.OfficeID && e.OrgID== OrgID).FirstOrDefault();
                    if (existingMapping != null )
                    {
                        existingMapping.IsActive = map.IsSelected;
                        existingMapping.CreateUser = map.CreateUser;
                        Update(existingMapping);
                    }
                    else
                    {
                        map.OrgID = OrgID;
                        map.EmployeeID = employee.EmployeeID;
                        map.IsActive = true;

                        Add(map);
                    }
                }
            }
        }
    }
}
