using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service
{
    public interface IProjectInfoService : IServiceBase<ProjectInfo>
    {


    }
    public class ProjectInfoService : IProjectInfoService
    {
        private readonly IProjectInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ProjectInfoService(IProjectInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<ProjectInfo> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.ProjectID);
            return entities;
        }

        public ProjectInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ProjectInfo Create(ProjectInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ProjectInfo objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
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

        public ProjectInfo Get(Expression<Func<ProjectInfo, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ProjectInfo> GetMany(Expression<Func<ProjectInfo, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
        public ProjectInfo GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

    }
}
