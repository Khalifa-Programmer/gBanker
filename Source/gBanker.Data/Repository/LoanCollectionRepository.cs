using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{

    public interface ILoanCollectionRepository : IRepository<DailyLoanTrx>
    {
        int delVoucher(Nullable<int> officeID, Nullable<System.DateTime> businessDate,int? OrgID);
        int setLoanAndSavingingLessFiftyPercent(int? OrgID, Nullable<int> officeID, Nullable<int> CenterID,Nullable<int> Qtype);
        IEnumerable<getDailyProduct_Result> getDailyProduct(Nullable<int> officeId, Nullable<int> orgId);
        IEnumerable<getDailyMember_Result> getDailyMember(Nullable<int> officeId, Nullable<int> orgId);
        IEnumerable<DailyLoanTrx> GetDailyLoanCollectionByProduct(int OrgId,int OfficeId);
        int UpdateSpecialLOan(Nullable<long> dailyLoanTrxID, Nullable<int> officeId, Nullable<int> centerId, Nullable<long> memberID, Nullable<int> productID, Nullable<int> lOanterm, Nullable<decimal> loanPaid, Nullable<decimal> intPaid, Nullable<decimal> totalPaid, Nullable<int> trxType, Nullable<int> orgID);
        IEnumerable<DailyLoanTrx> GetLoanCollectionDetailPaged(int centerId, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount);
    }

    public class LoanCollectionRepository : RepositoryBaseCodeFirst<DailyLoanTrx>, ILoanCollectionRepository
    {
       public LoanCollectionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

       //public IEnumerable<DailyLoanTrx> GetDailyLoanCollectionByCenter(int centerId)
       //{
       //    return 
       //}

       public int delVoucher(int? officeID, DateTime? businessDate,int? OrgID)
       {
           var officeIdParameter = new SqlParameter("@OfficeID", officeID);
           var date = new SqlParameter("@BusinessDate", businessDate);
           var orgidIdParameter = new SqlParameter("@OrgID", officeID);
           return DataContext.Database.ExecuteSqlCommand("delVoucher @OfficeID,@BusinessDate,@OrgID", officeIdParameter, date, orgidIdParameter);
       }


       public IEnumerable<DailyLoanTrx> GetLoanCollectionDetailPaged(int centerId, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount)
       {
           IQueryable<DailyLoanTrx> results = null;
           if (filterColumnName == "MemberCode" && !string.IsNullOrWhiteSpace(filterValue))
               results = DataContext.DailyLoanTrxes.Where(tr => tr.CenterID == centerId && tr.IsActive==true && (tr.TrxType == 10 || tr.TrxType == 11) && tr.MemberCode.Contains(filterValue)).OrderBy(tr => tr.MemberID);

           else if (filterColumnName == "ProductCode")

               results = DataContext.DailyLoanTrxes.Where(tr => tr.CenterID == centerId && tr.IsActive == true && (tr.TrxType == 10 || tr.TrxType == 11) && tr.ProductCode == filterValue).OrderBy(tr => tr.MemberID);
           else
               results = DataContext.DailyLoanTrxes.Where(tr => tr.CenterID == centerId && tr.IsActive == true && (tr.TrxType == 10 || tr.TrxType == 11)).OrderBy(tr => tr.MemberID);
           totalCount = results.LongCount();
           var obj = results.OrderBy(x => x.ProductCode).Skip(startRowIndex).Take(pageSize);
           return obj;


         
       }


       public int setLoanAndSavingingLessFiftyPercent(int? OrgID, int? officeID, int? CenterID, int? Qtype)
       {
           var officeIdParameter = new SqlParameter("@OfficeId", officeID);
           var CenterIDParameter = new SqlParameter("@CenterID", CenterID);
           var orgidIdParameter = new SqlParameter("@OrgId", OrgID);
           var QtypeParameter = new SqlParameter("@Qtype", Qtype);
           return DataContext.Database.ExecuteSqlCommand("setLoanAndSavingingLessFiftyPercent @OrgId,@OfficeId,@CenterID,@Qtype", orgidIdParameter, officeIdParameter, CenterIDParameter, QtypeParameter);
       }


       public IEnumerable<DailyLoanTrx> GetDailyLoanCollectionByProduct(int OrgId, int OfficeId)
       {
           var orgIdParameter = new SqlParameter("@OrgId", OrgId);
           var officeIdParameter = new SqlParameter("@OfficeId", OfficeId);

           return DataContext.Database.SqlQuery<DailyLoanTrx>("getDailyProduct @OfficeId,@OrgId",  officeIdParameter,orgIdParameter);

       }


       public IEnumerable<getDailyProduct_Result> getDailyProduct(int? officeId, int? orgId)
       {
           var orgIdParameter = new SqlParameter("@OrgId", orgId);
           var officeIdParameter = new SqlParameter("@OfficeId", officeId);

           return DataContext.Database.SqlQuery<getDailyProduct_Result>("getDailyProduct @OfficeId,@OrgId", officeIdParameter, orgIdParameter);
       }


       public IEnumerable<getDailyMember_Result> getDailyMember(int? officeId, int? orgId)
       {
           var orgIdParameter = new SqlParameter("@OrgId", orgId);
           var officeIdParameter = new SqlParameter("@OfficeId", officeId);

           return DataContext.Database.SqlQuery<getDailyMember_Result>("getDailyMember @OfficeId,@OrgId", officeIdParameter, orgIdParameter);
       }


       public int UpdateSpecialLOan(long? dailyLoanTrxID, int? officeId, int? centerId, long? memberID, int? productID, int? lOanterm, decimal? loanPaid, decimal? intPaid, decimal? totalPaid, int? trxType, int? orgID)
       {
           var dailyLoanTrxIDParameter = new SqlParameter("@DailyLoanTrxID", dailyLoanTrxID);
           var officeIdParameter = new SqlParameter("@OfficeId", officeId);
           var CenterIDParameter = new SqlParameter("@CenterId", centerId);
           var MemberIDParameter = new SqlParameter("@MemberID", memberID);
           var ProductIDParameter = new SqlParameter("@ProductID", productID);
           var LOantermParameter = new SqlParameter("@LOanterm", lOanterm);
           var LoanPaidParameter = new SqlParameter("@LoanPaid", loanPaid);
           var IntPaidParameter = new SqlParameter("@IntPaid", intPaid);
           var TotalPaidParameter = new SqlParameter("@TotalPaid", totalPaid);
           var TrxTypeParameter = new SqlParameter("@TrxType", trxType);
           var orgIDParameter = new SqlParameter("@orgID", orgID);


           return DataContext.Database.ExecuteSqlCommand("UpdateSpecialLOan @DailyLoanTrxID,@OfficeId,@CenterId,@MemberID,@ProductID,@LOanterm,@LoanPaid,@IntPaid,@TotalPaid,@TrxType,@orgID", dailyLoanTrxIDParameter, officeIdParameter, CenterIDParameter,MemberIDParameter,ProductIDParameter,LOantermParameter,LoanPaidParameter,IntPaidParameter,TotalPaidParameter,TrxTypeParameter,orgIDParameter);

       }
    }
}
