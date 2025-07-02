using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration;
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
   public interface IFileUploadService : IServiceBase<FileUpload>
    {
        IEnumerable<ValidationResult> IsValidFile(string FileName);        
    }
   public class FileUploadService : IFileUploadService
   {
       private readonly IFileUploadRepository repository;
       private readonly IUnitOfWorkCodeFirst unitOfWork;

       public FileUploadService(IFileUploadRepository repository, IUnitOfWorkCodeFirst unitOfWork)
       {
           this.repository = repository;
           this.unitOfWork = unitOfWork;
       }

       public IEnumerable<ValidationResult> IsValidFile(string FileName)
       {
           var entity = repository.Get(p => p.FileName == FileName);
           if (entity != null)
           {
               yield return new ValidationResult("FileName", "Duplicate File.");
           }
       }

       public IEnumerable<FileUpload> GetAll()
       {
           var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.FileName);
           return entities;
       }

       public FileUpload GetById(int id)
       {
           var entity = repository.GetById(id);
           return entity;
       }

       public FileUpload Create(FileUpload objectToCreate)
       {
           repository.Add(objectToCreate);
           Save();
           return objectToCreate;
       }

       public void Update(FileUpload objectToUpdate)
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

       public bool Inactivate(long id, DateTime? inactiveDate)
       {
           //throw new NotImplementedException();
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
           // throw new NotImplementedException();
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

       public void Save()
       {
           unitOfWork.Commit();
       }

       public FileUpload GetByIdLong(long id)
       {
           var entity = repository.GetById(id);
           return entity;
       }

        public IEnumerable<FileUpload> GetMany(Expression<Func<FileUpload, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
