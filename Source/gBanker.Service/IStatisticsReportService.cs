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
    public interface IStatisticsReportService : IServiceBase<StatisticsReport>
    { }
    public class StatisticsReportService : IStatisticsReportService
    {
        private readonly IStatisticsReportRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public StatisticsReportService(IStatisticsReportRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<StatisticsReport> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.StatisticsReportId);
            return entities;
        }

        public StatisticsReport GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public StatisticsReport Create(StatisticsReport objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(StatisticsReport objectToUpdate)
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
        public bool IsValidStatisticsReport(StatisticsReport StatisticsReport)
        {
            var entity = repository.Get(p => p.StatisticsReportId == StatisticsReport.StatisticsReportId);
            return entity == null ? true : false; ;
        }


        public IEnumerable<StatisticsReport> SearchStatisticsReport()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.StatisticsReportId);
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
        public StatisticsReport GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StatisticsReport> GetMany(Expression<Func<StatisticsReport, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}

