using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{


    public interface IEmployeeOfficeMappingService : IServiceBase<EmployeeOfficeMapping>
    {

        IEnumerable<EmployeeOfficeMapping> GetEmployeeOfficeMappings(string employeeCode);

        void CreateEmployeeOfficeMapping(int OrgID,string employeeCode, List<EmployeeOfficeMapping> mappings);

    }
    public class EmployeeOfficeMappingService : IEmployeeOfficeMappingService
    {
        private readonly IEmployeeOfficeMappingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public EmployeeOfficeMappingService(IEmployeeOfficeMappingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;

        }


        public IEnumerable<EmployeeOfficeMapping> GetEmployeeOfficeMappings(string employeeCode)
        {
            return repository.GetEmployeeOfficeMappings(employeeCode);
        }

        public IEnumerable<EmployeeOfficeMapping> GetAll()
        {
            throw new NotImplementedException();
        }

        public EmployeeOfficeMapping GetById(int id)
        {
            throw new NotImplementedException();
        }

        public EmployeeOfficeMapping Create(EmployeeOfficeMapping objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(EmployeeOfficeMapping objectToUpdate)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }


        public void CreateEmployeeOfficeMapping(int OrgID, string employeeCode, List<EmployeeOfficeMapping> mappings)
        {
            repository.CreateEmployeeOfficeMapping(OrgID,employeeCode, mappings);
            Save();
        }


        public EmployeeOfficeMapping GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<EmployeeOfficeMapping> GetMany(Expression<Func<EmployeeOfficeMapping, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
