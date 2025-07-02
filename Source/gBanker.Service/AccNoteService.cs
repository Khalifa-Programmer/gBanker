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
    public interface IAccNoteService : IServiceBase<AccNote>
    {
        IEnumerable<ValidationResult> IsValidNote(AccNote note);
        AccNote GetByNoteNo(int NoteNo);
        string GetNoteDetail(int note_Id);
    }
    public class AccNoteService : IAccNoteService
    {
        private readonly IAccNoteRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AccNoteService(IAccNoteRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AccNote> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.NoteName);
            return entities;
        }

        public AccNote GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public AccNote GetByNoteNo(int NoteNo)
        {
            var entity = repository.Get(p => p.NoteNo == NoteNo);
            return entity;
        }

        public string GetNoteDetail(int note_Id)
        {
            var result = repository.GetById(note_Id);
            var note = "";
            if (result != null)
                note = result.NoteNo.ToString(); //+ " - " + result.NoteName;
            return note;
            
        }

        public AccNote Create(AccNote objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AccNote objectToUpdate)
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
        IEnumerable<ValidationResult> IAccNoteService.IsValidNote(AccNote note)
        {
            var entity = repository.Get(p => p.NoteNo == note.NoteNo);
            if (entity != null)
            {
                yield return new ValidationResult("NoteNo", "Duplicate Note Number.");

            }
        }



        public AccNote GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<AccNote> GetMany(Expression<Func<AccNote, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
