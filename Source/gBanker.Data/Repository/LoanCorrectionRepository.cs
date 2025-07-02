using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{

    public interface ILoanCorrectionRepository : IRepository<LoanCorrectionTrx>
    {
        IEnumerable<proc_get_LoanCorrection_Result> GetLoanCorrectionDetail(int? orgID, int? officeID, DateTime? collectionDate);
        int SpecialCollection(int? officeID, int? centerID, int? productID, int? memberID, int? loanTerm, string collectionDay, DateTime? collectionDate, int? qType, int? transType, Nullable<decimal> loanPaid, Nullable<decimal> intPaid);
        int LoanCorrection(int? OrgID,int? officeID, int? centerID, int? productID, int? memberID, int? loanTerm, string collectionDay, DateTime? collectionDate, int? qType, int? transType, Nullable<decimal> loanPaid, Nullable<decimal> intPaid);

       
        int delVoucher(Nullable<int> officeID, Nullable<System.DateTime> businessDate);

    }
    public class LoanCorrectionRepository : RepositoryBaseCodeFirst<LoanCorrectionTrx>, ILoanCorrectionRepository
    {
        public LoanCorrectionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }




        public int SpecialCollection(int? officeID, int? centerID, int? productID, int? memberID, int? loanTerm, string collectionDay, DateTime? collectionDate, int? qType, int? transType, decimal? loanPaid, decimal? intPaid)
        {
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var centerIDParameter = new SqlParameter("@CenterID", centerID);
            var productIDParameter = new SqlParameter("@ProductID", productID);
            var memberIDParameter = new SqlParameter("@MemberID", memberID);
            var loanTermParameter = new SqlParameter("@LoanTerm", loanTerm);
            var collectionDayParameter = new SqlParameter("@CollectionDay", collectionDay);
            var collectionDateParameter = new SqlParameter("@CollectionDate", collectionDate);
            var qTypeParameter = new SqlParameter("@qType", qType);
            var transTypeParameter = new SqlParameter("@TransType", transType);
            var loanPaidParameter = new SqlParameter("@LoanPaid", loanPaid);
            var intPaidParameter = new SqlParameter("@intPaid", intPaid);
            return DataContext.Database.ExecuteSqlCommand("SpecialCollection @OfficeID,@CenterID,@ProductID,@MemberID,@LoanTerm,@CollectionDay,@CollectionDate,@qType,@TransType,@LoanPaid,@intPaid", officeIdParameter, centerIDParameter, productIDParameter, memberIDParameter, loanTermParameter, collectionDayParameter, collectionDateParameter, qTypeParameter, transTypeParameter, loanPaidParameter, intPaidParameter);

        }


        public int delVoucher(int? officeID, DateTime? businessDate)
        {
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var date = new SqlParameter("@BusinessDate", businessDate);
            return DataContext.Database.ExecuteSqlCommand("delVoucher @OfficeID,@BusinessDate", officeIdParameter, date);
        }


        public int LoanCorrection(int? OrgID,int? officeID, int? centerID, int? productID, int? memberID, int? loanTerm, string collectionDay, DateTime? collectionDate, int? qType, int? transType, decimal? loanPaid, decimal? intPaid)
        {
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var centerIDParameter = new SqlParameter("@CenterID", centerID);
            var productIDParameter = new SqlParameter("@ProductID", productID);
            var memberIDParameter = new SqlParameter("@MemberID", memberID);
            var loanTermParameter = new SqlParameter("@LoanTerm", loanTerm);
            var collectionDayParameter = new SqlParameter("@CollectionDay", collectionDay);
            var collectionDateParameter = new SqlParameter("@CollectionDate", collectionDate);
            var qTypeParameter = new SqlParameter("@qType", qType);
            var transTypeParameter = new SqlParameter("@TransType", transType);
            var loanPaidParameter = new SqlParameter("@LoanPaid", loanPaid);
            var intPaidParameter = new SqlParameter("@intPaid", intPaid);
            return DataContext.Database.ExecuteSqlCommand("LoanCorrection @OrgID,@OfficeID,@CenterID,@ProductID,@MemberID,@LoanTerm,@CollectionDay,@CollectionDate,@qType,@TransType,@LoanPaid,@intPaid",orgIdParameter, officeIdParameter, centerIDParameter, productIDParameter, memberIDParameter, loanTermParameter, collectionDayParameter, collectionDateParameter, qTypeParameter, transTypeParameter, loanPaidParameter, intPaidParameter);

        }

        public IEnumerable<proc_get_LoanCorrection_Result> GetLoanCorrectionDetail(int? orgID, int? officeID, DateTime? collectionDate)
        {
            var orgIdParameter = new SqlParameter("@OrgId", orgID);
            var officeIdParameter = new SqlParameter("@officeId", officeID);
            var collectionDateParameter = new SqlParameter("@TrxDate", collectionDate);
            return DataContext.Database.SqlQuery<proc_get_LoanCorrection_Result>("proc_get_LoanCorrection @OrgId, @officeId,@TrxDate",orgIdParameter, officeIdParameter, collectionDateParameter);

        }
    }
}
