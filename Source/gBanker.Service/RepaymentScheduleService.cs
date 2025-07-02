using gBanker.Core.Common;
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
    public interface IRepaymentScheduleService : IServiceBase<RepaymentSchedule>
    {
        //IEnumerable<DBRepaymentScheduleDetailModel> GetRePaymentDetail(int? officeID, int? memberID, int? prodID, string filterColumnName, string filterValue, string TypeFilterColumn, int startRowIndex, string jtSorting, int pageSize, out long TotCount);
        IEnumerable<DBRepaymentScheduleDetailModel> GetRePaymentDetail(int? officeID, int? memberID, int? prodID);
        IEnumerable<GetRepaymentSchedule_Result> GetRepaymentSchedule(int? officeId, int? memberID, int? productId);
    }
    public class RepaymentScheduleService : IRepaymentScheduleService
    {
        private readonly IRepaymentScheduleRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public RepaymentScheduleService(IRepaymentScheduleRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<DBRepaymentScheduleDetailModel> GetRePaymentDetail(int? officeID, int? memberID, int? prodID)
        {
            //return IEnumerable<DBRepaymentScheduleDetailModel> GetRePaymentDetail(int? officeID, int? memberID, int? prodID);
            return repository.GetRePaymentDetail(officeID, memberID, prodID);
        }

        public IEnumerable<RepaymentSchedule> GetAll()
        {
            throw new NotImplementedException();
        }

        public RepaymentSchedule GetById(int id)
        {
            throw new NotImplementedException();
        }

        public RepaymentSchedule Create(RepaymentSchedule objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(RepaymentSchedule objectToUpdate)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
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

        public void Save()
        {
            throw new NotImplementedException();
        }


        public IEnumerable<GetRepaymentSchedule_Result> GetRepaymentSchedule(int? officeId, int? memberID, int? productId)
        {
            return repository.GetRepaymentSchedule(officeId, memberID, productId);
        }


        public RepaymentSchedule GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<RepaymentSchedule> GetMany(Expression<Func<RepaymentSchedule, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
