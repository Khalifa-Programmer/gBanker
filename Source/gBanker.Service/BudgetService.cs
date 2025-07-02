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
    public interface IBudgetService : IServiceBase<Budget>
    {
        //IEnumerable<ValidationResult> IBudgetService.IsValidBudget(Budget budget);
        //AccNote GetByNoteNo(int NoteNo);
        //string GetNoteDetail(int note_Id);
    }
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public BudgetService(IBudgetRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Budget> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.BudgetID);
            return entities;
        }

        public Budget GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        //public Budget GetByNoteNo(int NoteNo)
        //{
        //    var entity = repository.Get(p => p.NoteNo == NoteNo);
        //    return entity;
        //}

        //public string GetNoteDetail(int note_Id)
        //{
        //    var result = repository.GetById(note_Id);
        //    var note = "";
        //    if (result != null)
        //        note = result.NoteNo.ToString(); //+ " - " + result.NoteName;
        //    return note;

        //}

        public Budget Create(Budget objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Budget objectToUpdate)
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
        //IEnumerable<ValidationResult> IBudgetService.IsValidBudget(Budget budget)
        //{
            //var entity = repository.Get(p => p.NoteNo == note.NoteNo);
            //if (entity != null)
            //{
            //    yield return new ValidationResult("NoteNo", "Duplicate Note Number.");

            //}
            //throw new NotImplementedException();
        //}

        public Budget GetByIdLong(long id)
        {
            //var entity = repository.GetById(id);
            //return entity;
            throw new NotImplementedException();
        }

        public IEnumerable<Budget> GetMany(Expression<Func<Budget, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
