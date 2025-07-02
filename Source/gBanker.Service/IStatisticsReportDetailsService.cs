using gBanker.Data.CodeFirstMigration;
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
    public interface IStatisticsReportDetailsService : IServiceBase<StatisticsReportDetails>
    { }
    public class StatisticsReportDetailsService : IStatisticsReportDetailsService
    {
        private readonly IStatisticsReportDetailsRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public StatisticsReportDetailsService(IStatisticsReportDetailsRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<StatisticsReportDetails> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.StatisticsReportDetailsID);
            return entities;
        }

        public StatisticsReportDetails GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public StatisticsReportDetails Create(StatisticsReportDetails objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(StatisticsReportDetails objectToUpdate)
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
        public bool IsValidStatisticsReportDetails(StatisticsReportDetails StatisticsReportDetails)
        {
            var entity = repository.Get(p => p.StatisticsReportDetailsID == StatisticsReportDetails.StatisticsReportDetailsID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<StatisticsReportDetails> SearchStatisticsReportDetails()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.StatisticsReportDetailsID);
        }


        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                //obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
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
                if (isActive == true)
                {
                    return false;
                }
            }

            return true;
        }
        public StatisticsReportDetails GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StatisticsReportDetails> GetMany(Expression<Func<StatisticsReportDetails, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
