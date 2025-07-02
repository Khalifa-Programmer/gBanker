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
    public interface IDashBoardService : IServiceBase<DashBoard>
    {
        DashBoard GetPieDataByOfficeId(int officeId);
        //AccNote GetByNoteNo(int NoteNo);
        //string GetNoteDetail(int note_Id);
    }
    public class DashBoardService : IDashBoardService
    {
        private readonly IDashBoardRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DashBoardService(IDashBoardRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DashBoard> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.DashBoardID);
            return entities;
        }

        public DashBoard GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DashBoard GetPieDataByOfficeId(int officeId)
        {
            var entity = repository.Get(w=> w.OfficeID==officeId && w.ItemCode == null);
            return entity;
        }
        //public DashBoard GetBarDataByOfficeId(int officeId)
        //{
        //    var entity = repository.Get(w => w.OfficeID == officeId && w.ItemCode == null);
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

        public DashBoard Create(DashBoard objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DashBoard objectToUpdate)
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



        public DashBoard GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DashBoard> GetMany(Expression<Func<DashBoard, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
