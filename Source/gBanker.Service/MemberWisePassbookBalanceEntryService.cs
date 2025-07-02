using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data;
using BasicDataAccess;
using BasicDataAccess.Data;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.DBDetailModels;

namespace gBanker.Service
{
    public interface IMemberWisePassbookBalanceEntryService : IServiceBase<MemberWisePassbookBalanceEntry>
    {
        IEnumerable<PassbookEntryModel> GetMemberWisePassbookBalanceEntryList();
        void DeleteById(int id);
    }
    public class MemberWisePassbookBalanceEntryService : IMemberWisePassbookBalanceEntryService
    {
        private readonly IMemberWisePassbookBalanceEntryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
       

        public MemberWisePassbookBalanceEntryService(IMemberWisePassbookBalanceEntryRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
          
        }
       
        public MemberWisePassbookBalanceEntry GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public MemberWisePassbookBalanceEntry Create(MemberWisePassbookBalanceEntry objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(MemberWisePassbookBalanceEntry objectToUpdate)
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
        public void DeleteById(int id)
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
    
        public IEnumerable<Area> GetMany(Expression<Func<Area, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductXEmploymentProductMapping> GetAll()
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

        public ProductXEmploymentProductMapping GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MemberWisePassbookBalanceEntry> GetMany(Expression<Func<MemberWisePassbookBalanceEntry, bool>> where)
        {
            throw new NotImplementedException();
        }

        IEnumerable<MemberWisePassbookBalanceEntry> IServiceBase<MemberWisePassbookBalanceEntry>.GetAll()
        {
            throw new NotImplementedException();
        }

        MemberWisePassbookBalanceEntry IServiceBase<MemberWisePassbookBalanceEntry>.GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<PassbookEntryModel> GetMemberWisePassbookBalanceEntryList()
        {
            try
            {
                return repository.GetMemberWisePassbookBalanceEntryList();
            }
            catch (Exception ex)
            {
                return new List<PassbookEntryModel>();
            }
        }

    }
}
