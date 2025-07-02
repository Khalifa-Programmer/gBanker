using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration.Db;
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
    public interface IDailySavingTrxService : IServiceBase<DailySavingTrx>
    {
        //IEnumerable<DailySavingTrx> Get_SavingSummaryInfo(Nullable<int> orgID, Nullable<int> officeID, Nullable<int> centerId, Nullable<long> memberID, Nullable<int> productID, Nullable<int> Savingterm);
        IEnumerable<DailySavingTrx> GetDailySavingTrxDetailPaged(string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount, int? OrgID, int? OfficeID);
        DailySavingTrx IsDataExistByDate(DateTime date);

    }
    public class DailySavingTrxService : IDailySavingTrxService
     {
        private readonly IDailySavingTrxRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DailySavingTrxService(IDailySavingTrxRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DailySavingTrx> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == 1).OrderBy(c => c.MemberID);
            return entities;
        }

        public DailySavingTrx GetById(int id)
        {
            //throw new NotImplementedException();
            var entity = repository.GetById(id);
            return entity;
        }

        public DailySavingTrx Create(DailySavingTrx objectToCreate)
        {
            //throw new NotImplementedException();
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DailySavingTrx objectToUpdate)
        {
            //throw new NotImplementedException();
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            //throw new NotImplementedException();
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
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
                obj.IsActive = 0;
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
                if (isActive == 1)
                {
                    return false;
                }
            }

            return true;
        }
        public IEnumerable<DailySavingTrx> GetDailySavingTrxDetailPaged(string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount, int? OrgID, int? OfficeID)
        {
            return repository.GetDailySavingTrxDetailPaged(filterColumnName, filterValue, startRowIndex, pageSize, out totalCount, OrgID,OfficeID);
        }
        public DailySavingTrx GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<DailySavingTrx> GetMany(Expression<Func<DailySavingTrx, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == 1);
            return entities;
        }
        //public IEnumerable<DailySavingTrx> Get_SavingSummaryInfo(int? officeID, int? centerId, long? memberID, int? productID, int? Savingterm)
        //{
        //    return repository.GetAll();

        //}
        public DailySavingTrx IsDataExistByDate(DateTime date)
        {
            var entity = repository.IsDataExistByDate(date);
            return entity;
        }
    }
}
