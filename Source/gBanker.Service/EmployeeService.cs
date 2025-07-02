using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
//using gBanker.Data.CodeFirstMigration.Db;
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
    public interface IEmployeeService : IServiceBase<Employee>
    {
        IEnumerable<EmployeeDetail> getEmployeeList(int? officeID, string EmpID);

        IEnumerable<ValidationResult> IsValidEmployee(Employee employee);
        IEnumerable<Employee> SearchEmployee();
        IEnumerable<ValidationResult> CheckDupliEmployee(string EmpCode);
        IEnumerable<ValidationResult> CheckDupliEmployee(string EmpCode,int EmpID);
        // bool IsContinued(int id);
        //IEnumerable<getEmployeeInfo_Result> getEmployee();
        Employee GetByCode(string employeeCode);
        IEnumerable<Employee> SearchEmployeeByOffice(int officeID);

    }
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public EmployeeService(IEmployeeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Employee> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.EmployeeID);
            return entities;
        }

        public Employee GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Employee GetByCode(string employeeCode)
        {
            var entity = repository.Get(e => e.EmployeeCode == employeeCode.Trim() && e.IsActive==true && e.EmployeeStatus==1);
            return entity;
        }
        public Employee Create(Employee objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Employee objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<Employee> SearchEmployee()
        {
            return repository.GetMany(g => g.IsActive == true || g.EmployeeStatus==1).OrderBy(g => g.EmployeeID);
        }

       
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }


        public bool IsContinued(long id)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == false)
                {
                    return false;
                }
            }

            return true;
        }



        IEnumerable<ValidationResult> IEmployeeService.IsValidEmployee(Employee employee)
        {
            var entity = repository.Get(p => p.EmployeeCode == employee.EmployeeCode && p.EmployeeStatus==1 && p.IsActive==true);
            if (entity != null)
            {

                yield return new ValidationResult("EmployeeCode", "Duplicate Employee.");

            }
        }


        public Employee GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<EmployeeDetail> getEmployeeList(int? officeID, string EmpID)
        {
            return repository.getEmployeeList( officeID, EmpID);

        }

        public IEnumerable<ValidationResult> CheckDupliEmployee(string EmpCode)
        {
            var entity = repository.Get(p => ( p.IsActive == true) && p.EmployeeCode == EmpCode);

            //msg = "test";  
            if (entity != null)
            {
                yield return new ValidationResult("Employee", "Duplicate EmployeeCode .");
            }
        }

        public IEnumerable<ValidationResult> CheckDupliEmployee(string EmpCode, int EmpID)
        {
            var entity = repository.Get(p => (p.IsActive == true) && p.EmployeeCode == EmpCode && p.EmployeeID != EmpID && p.EmployeeStatus == 1);

            //msg = "test";  
            if (entity != null)
            {
                yield return new ValidationResult("Employee", "Duplicate EmployeeCode .");
            }
        }

        public IEnumerable<Employee> GetMany(Expression<Func<Employee, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public IEnumerable<Employee> SearchEmployeeByOffice(int officeID)
        {
            
            return repository.GetMany(g => g.IsActive == true && g.OfficeID==officeID).OrderBy(g => g.EmployeeID);
        }
    }
}
