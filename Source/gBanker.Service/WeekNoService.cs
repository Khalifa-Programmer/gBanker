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
    public interface IWeekNoService : IServiceBase<WeekNo>
    {
        
    }
    public class WeekNoService : IWeekNoService
    {
        private readonly IWeekNoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public WeekNoService(IWeekNoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<WeekNo> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.WeekNoSl);
            return entities;
        }

        public WeekNo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public WeekNo Create(WeekNo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(WeekNo objectToUpdate)
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
        //IEnumerable<ValidationResult> IAccNoteService.IsValidNote(AccNote note)
        //{
        //    var entity = repository.Get(p => p.NoteNo == note.NoteNo);
        //    if (entity != null)
        //    {
        //        yield return new ValidationResult("NoteNo", "Duplicate Note Number.");

        //    }
        //}

        public WeekNo GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WeekNo> GetMany(Expression<Func<WeekNo, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
