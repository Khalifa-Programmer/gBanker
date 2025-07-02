using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using gBanker.Data.CodeFirstMigration;

namespace gBanker.Service
{
    public interface IProcessInfoService : IServiceBase<ProcessInfo>
    {
        ProcessInfo GetByOfficeId(int Office_id);
        DateTime GetInitialDtByOfficeId(int Office_id);
        ProcessInfo CheckClosingDtByOfficeId(int Office_id);
    }
    public class ProcessInfoService : IProcessInfoService
    {
        private readonly IProcessInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ProcessInfoService(IProcessInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ProcessInfo> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.ProcessInfoID);
            return entities;
        }

        public ProcessInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ProcessInfo GetByOfficeId(int Office_id)
        {
            var entity = repository.GetAll().Where(g => g.OfficeID == Office_id && g.ClosingStatus==false).OrderByDescending(o => o.ProcessInfoID).First();
            return entity;
        }
        public DateTime GetInitialDtByOfficeId(int Office_id)
        {
            var entity = repository.GetMany(g => g.OfficeID == Office_id && g.ClosingStatus == false).OrderByDescending(o => o.ProcessInfoID).First();
            if(entity != null)
                return entity.InitialDate;
            else { 
                var entity1 = repository.GetMany(g => g.OfficeID == Office_id).OrderByDescending(o => o.ProcessInfoID).First();
                return entity1.InitialDate;
            }
        }
        public ProcessInfo CheckClosingDtByOfficeId(int Office_id)
        {
            var entity = repository.GetMany(g => g.OfficeID == Office_id && g.ClosingStatus == false).OrderByDescending(o => o.ProcessInfoID).FirstOrDefault();
            return entity;
        }
        public ProcessInfo Create(ProcessInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ProcessInfo objectToUpdate)
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
            //throw new NotImplementedException();
            unitOfWork.Commit();
        }
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }



        public ProcessInfo GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<ProcessInfo> GetMany(Expression<Func<ProcessInfo, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
