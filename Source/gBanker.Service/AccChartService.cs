using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface IAccChartService : IServiceBase<AccChart>
    {
        bool IsValidAccCode(string accCode);
        AccChart GetByAccCode(string accCode);
        AccChart GetByAccID(int accCode);
        AccChart GetByAccCodeSecondLevel(string accCode);
        IEnumerable<AccChart> GetChartDetail(string filterColumnName, string filterValue);
        IEnumerable<DBAccChartDetailModel> GetAccChartDetail(int? orgId, string filterColumnName, string filterValue, out long TotCount);
    }
    public class AccChartService : IAccChartService
    {
        private readonly IAccChartRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AccChartService(IAccChartRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AccChart> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.AccCode);
            return entities;
        }

        public AccChart GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public AccChart GetByAccCode(string accCode)
        {
            var entity = repository.Get(p => p.AccCode == accCode);
            return entity;
        }

        public AccChart Create(AccChart objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AccChart objectToUpdate)
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
        public bool IsValidAccCode(string accCode)
        {
            var entity = repository.Get(p => p.AccCode == accCode && p.IsActive==true);
            return entity == null ? true : false;
        }
        public IEnumerable<AccChart> GetChartDetail(string filterColumnName, string filterValue)
        {
            IEnumerable<AccChart> results = null;
            if (filterColumnName == "AccCode")
                results = repository.GetAll().Where(w => w.IsActive == true && w.AccCode.Contains(filterValue)).OrderBy(w=>w.AccCode);
            else if (filterColumnName == "AccName")
                results = repository.GetAll().Where(w => w.IsActive == true && w.AccName.Contains(filterValue)).OrderBy(w => w.AccCode);
            else
                results = repository.GetAll().Where(w => w.IsActive == true).OrderBy(w => w.AccCode);
            return results;
        }
        public IEnumerable<DBAccChartDetailModel> GetAccChartDetail(int? orgId, string filterColumnName, string filterValue, out long TotCount)
        {
            return repository.GetAccChartDetail(orgId,filterColumnName, filterValue, out TotCount);
        }



        public AccChart GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }


        public AccChart GetByAccCodeSecondLevel(string accCode)
        {
            var entity = repository.Get(p => p.SecondLevel == accCode);
            return entity;
        }


        public AccChart GetByAccID(int accCode)
        {
            var entity = repository.Get(p => p.AccID ==accCode);
            return entity;
        }

        public IEnumerable<AccChart> GetMany(Expression<Func<AccChart, bool>> x)
        {
            var entities = repository.GetMany(x).Where(b => b.IsActive == true);
            return entities;
        }
    }
}
