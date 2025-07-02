using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using gBankerCodeFirstMigration.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface IOrganizationService : IServiceBase<Organization>
    {
        IEnumerable<Organization> GetOrganizationDetailPaged(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long totalCount, int? OrgID);
        List<Organization> GetOrganizationDetails<TParamOType>(TParamOType target);
        Task<Organization> GetOrganizationById(long id);
        Task<bool> UpdateOrganization(Organization entity);
    }
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public OrganizationService(IOrganizationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }


        public async Task<Organization> GetOrganizationById(long id)
        {
            try
            {
                return await repository.GetOrganizationById(id);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateOrganization(Organization entity)
        {
            try
            {               
                return await repository.UpdateOrganization(entity);
            }
            catch
            {
                return false;
            }
        }

        public Organization Create(Organization objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public IEnumerable<Organization> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.OrganizationCode);
            return entities;
        }

        public Organization GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Organization GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<Organization> GetMany(Expression<Func<Organization, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Organization> GetOrganizationDetailPaged(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long totalCount, int? OrgID)
        {
            return repository.GetOrganizationDetailPaged(filterColumnName, filterValue, startRowIndex, jtSorting, pageSize, out totalCount, OrgID);
        }

        public List<Organization> GetOrganizationDetails<TParamOType>(TParamOType target)
        {
            var sql = "Proc_get_OrgDetails @OrgID";
            return repository.GetSqlResult<Organization, TParamOType>(sql, target);
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == true)
                {
                    return false;
                }
            }

            return true;
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public void Update(Organization objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }
    }
}
