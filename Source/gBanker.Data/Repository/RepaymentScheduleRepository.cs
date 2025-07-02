using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IRepaymentScheduleRepository : IRepository<RepaymentSchedule>
    {
        IEnumerable<DBRepaymentScheduleDetailModel> GetRePaymentDetail(int? officeID, int? memberID, int? prodID);
        IEnumerable<GetRepaymentSchedule_Result> GetRepaymentSchedule(int? officeId, int? memberID, int? productId);
    }
    public class RepaymentScheduleRepository : RepositoryBaseCodeFirst<RepaymentSchedule>, IRepaymentScheduleRepository
    {
        public RepaymentScheduleRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<DBRepaymentScheduleDetailModel> GetRePaymentDetail(int? officeID, int? memberID, int? prodID)
        {
            IQueryable<RepaymentSchedule> results = null;
            results = DataContext.RepaymentSchedules.Where(x => x.IsActive == true && x.OfficeID == officeID && x.MemberID == memberID && x.ProductID == prodID);
            var obj = results.OrderBy(o => o.MemberID).Select(s => new DBRepaymentScheduleDetailModel()
            {
                RepaymentScheduleID = s.RepaymentScheduleID,
                LoanSummaryID = s.LoanSummaryID,
                OfficeID = s.OfficeID,
                MemberID = s.MemberID,
                ProductID = s.ProductID,
                CenterID = s.CenterID,
                CenterCode = s.Center.CenterCode,
                CenterName = s.Center.CenterCode + ", " + s.Center.CenterName,
                FullName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName,
                MemberCategoryID = s.MemberCategoryID,
                LoanTerm = s.LoanTerm,
                RepaymentDate = s.RepaymentDate,
                InstallmentNo =Convert.ToInt16(s.InstallmentNo),

                ProductCode = s.Product.ProductCode,
                ProductName = s.Product.ProductCode,

                LoanInstallment=s.LoanInstallment,
                IntInstallment=s.IntInstallment,

                CreateUser=s.CreateUser
                // InstallmentNo = s.LoanTrx.InstallmentNo,
                //InstallmentDate = s.LoanSummary.LoanTrx.InstallmentDate

            });
            return obj.OrderBy(o => o.MemberID);
        }


        public IEnumerable<GetRepaymentSchedule_Result> GetRepaymentSchedule(int? officeId, int? memberID, int? productId)
        {
            var officeIdParameter = new SqlParameter("@officeId", officeId);
            var memberIDParameter = new SqlParameter("@MemberID", memberID);
            var ProductIdParameter = new SqlParameter("@ProductId", productId);

            return DataContext.Database.SqlQuery<GetRepaymentSchedule_Result>("GetRepaymentSchedule @officeId,@MemberID,@ProductId", officeIdParameter, memberIDParameter, ProductIdParameter);

        }
    }
}