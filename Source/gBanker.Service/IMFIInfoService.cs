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
    public interface IMFIInfoService : IServiceBase<MFIInformation>
    { }
    public class MFIInfoService : IMFIInfoService
    {
        private readonly IMFIInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public MFIInfoService(IMFIInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<MFIInformation> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.MFIId);
            return entities;
        }

        public MFIInformation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public MFIInformation Create(MFIInformation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(MFIInformation objectToUpdate)
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
        public bool IsValidMFIInfo(MFIInformation MFIInfo)
        {
            var entity = repository.Get(p => p.MFIId == MFIInfo.MFIId);
            return entity == null ? true : false; ;
        }


        public IEnumerable<MFIInformation> SearchMFIInfo()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.MFIId);
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


        public MFIInformation GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MFIInformation> GetMany(Expression<Func<MFIInformation, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
