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
    public interface ISpecialLoanCollectionRepository : IRepository<DailyLoanTrx>
    {
        IEnumerable<Proc_get_MaxLoanTerm_Result> Get_MaxLoanTerm(Nullable<int> orgID, Nullable<int> officeID, Nullable<int> centerId, Nullable<long> memberID, Nullable<int> productID);

        IEnumerable<proc_get_SpecialLoanCollection_Result> GetSpecialLoanCollectionDetail(int? officeID, string vday);
        int SetSLCTxtKeyPress(int? orgID, int? officeID, int? centerID, int? productID, long? memberID, int? loanTerm,  DateTime? collectionDate,  int? transType);
        int SpecialCollection(int? OrgID,int? officeID, int? centerID, int? productID, long? memberID, int? loanTerm, string collectionDay, DateTime? collectionDate, int? qType, int? transType, Nullable<decimal> loanPaid, Nullable<decimal> intPaid);
        int LoanCorrection(int? orgID, int? officeID, int? centerID, int? productID, long? memberID, int? loanTerm, string collectionDay, DateTime? collectionDate, int? qType, int? transType, Nullable<decimal> loanPaid, Nullable<decimal> intPaid);
        int UpdateSpecialLOan(Nullable<long> dailyLoanTrxID, Nullable<int> officeId, Nullable<int> centerId, Nullable<long> memberID, Nullable<int> productID, Nullable<int> lOanterm, Nullable<decimal> loanPaid, Nullable<decimal> intPaid, Nullable<decimal> totalPaid, Nullable<int> trxType, Nullable<int> orgID);

        int MaxLoanTerm(DailyLoanTrx loansummary);
        int delVoucher( Nullable<int> officeID, Nullable<System.DateTime> businessDate,Nullable<int> OrgID);
        IEnumerable<proc_get_SpecialLoanCollection_Result> GetSpecialLoanCollectionDetail(int? OrgID,int? officeID, string vday, string filterColumnName, string filterValue);
        IEnumerable<proc_get_SpecialLoanCollection_Result> GetSpecialLoanCollectionDetailReabte(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue);

        IEnumerable<GetSetSLCTxtKeyPress_Result> GetSetSLCTxtKeyPress(int? orgID, int? officeID, int? centerID, int? productID, long? memberID, int? loanTerm, DateTime? collectionDate, int? transType);
        IEnumerable<proc_get_SpecialLoanCollection_Result> GetSpecialLoanCollectionDetailEmpWise(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue,int?EmpID);
        IEnumerable<proc_get_SpecialLoanCollection_Result> GetRebateDetails(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue);
       
    }
    public class SpecialLoanCollectionRepository: RepositoryBaseCodeFirst<DailyLoanTrx>, ISpecialLoanCollectionRepository
    {
        public SpecialLoanCollectionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


        //public IEnumerable<DbSpecialLoanCollectionDetailModel> GetSpecialLoanCollectionDetail()
        //{
        //    var obj = DataContext.DailyLoanTrxes.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) )
        //      .Select(s => new DbSpecialLoanCollectionDetailModel()
        //      {
        //          DailyLoanTrxID=s.DailyLoanTrxID,
        //          LoanSummaryID = s.LoanSummaryID,
        //          OfficeID = s.OfficeID,
        //          OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
        //          OfficeName = s.Office == null ? "" : s.Office.OfficeName,
        //          CenterCode = s.Center.CenterCode,
        //          MemberCode = s.Member.MemberCode,
        //          MemberName = s.Member.FirstName,
        //          ProductCode = s.Product.ProductCode,
        //          LoanTerm = s.LoanTerm,
        //          PrincipalLoan = s.PrincipalLoan,
        //          LoanRepaid = s.LoanRepaid,
        //          IntCharge = s.IntCharge,
        //          InstallmentNo = s.InstallmentNo,
        //          Advance = s.Advance,
        //          TotalPaid=s.TotalPaid,
        //          LoanDue = s.LoanDue,
        //          LoanPaid=s.LoanPaid,
        //          IntDue=s.IntDue,
        //          IntPaid = s.IntPaid


        //      });

        //    return obj;
        //}


        public int MaxLoanTerm(DailyLoanTrx loansummary)
        {
            var obj = DataContext.LoanSummaries.Where(x => x.OfficeID == loansummary.OfficeID && x.CenterID == loansummary.CenterID && x.MemberID == loansummary.MemberID && x.ProductID == loansummary.ProductID)
               .Max(u => (int?)u.LoanTerm) ?? 0;
            return Convert.ToInt16(obj);
        }



        public IEnumerable<proc_get_SpecialLoanCollection_Result> GetSpecialLoanCollectionDetail(int? officeID, string vday)
        {
            var officeIdParameter = new SqlParameter("@officeId", officeID);
            var vcolday = new SqlParameter("@CollectionDay", vday);
            return DataContext.Database.SqlQuery<proc_get_SpecialLoanCollection_Result>("proc_get_SpecialLoanCollection @officeId,@CollectionDay", officeIdParameter, vcolday);
        }


        public IEnumerable<proc_get_SpecialLoanCollection_Result> GetSpecialLoanCollectionDetail(int? OrgID,int? officeID, string vday, string filterColumnName, string filterValue)
        {
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@officeId", officeID);
            var vcolday = new SqlParameter("@CollectionDay", vday);
            var filColName = new SqlParameter("@filterColumnName", filterColumnName);
            var filColvalue = new SqlParameter("@filterValue", filterValue);
            return DataContext.Database.SqlQuery<proc_get_SpecialLoanCollection_Result>("proc_get_SpecialLoanCollection @OrgID,@officeId,@CollectionDay,@filterColumnName,@filterValue",orgIdParameter, officeIdParameter, vcolday, filColName, filColvalue);
        }

        public int SpecialCollection(int? OrgID, int? officeID, int? centerID, int? productID, long? memberID, int? loanTerm, string collectionDay, DateTime? collectionDate, int? qType, int? transType, decimal? loanPaid, decimal? intPaid)
        {
            var OrgIDParaMeter=new SqlParameter("@OrgID",OrgID);
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
            return DataContext.Database.ExecuteSqlCommand("SpecialCollection @OrgID,@OfficeID,@CenterID,@ProductID,@MemberID,@LoanTerm,@CollectionDay,@CollectionDate,@qType,@TransType,@LoanPaid,@intPaid", OrgIDParaMeter, officeIdParameter, centerIDParameter, productIDParameter, memberIDParameter, loanTermParameter, collectionDayParameter, collectionDateParameter, qTypeParameter, transTypeParameter, loanPaidParameter, intPaidParameter);

        }
        

        public int delVoucher( int? officeID, DateTime? businessDate,int? OrgID)
        {
           
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var date = new SqlParameter("@BusinessDate", businessDate);
            var orgIDParameter = new SqlParameter("@OrgID", OrgID);
            return DataContext.Database.ExecuteSqlCommand("delVoucher @OfficeID,@BusinessDate,@OrgID", officeIdParameter, date,orgIDParameter);
        }


        public int LoanCorrection(int? orgID, int? officeID, int? centerID, int? productID, long? memberID, int? loanTerm, string collectionDay, DateTime? collectionDate, int? qType, int? transType, decimal? loanPaid, decimal? intPaid)
        {
            var orgIdParameter = new SqlParameter("@OrgID", orgID);
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
            return DataContext.Database.ExecuteSqlCommand("LoanCorrection @OrgID,@OfficeID,@CenterID,@ProductID,@MemberID,@LoanTerm,@CollectionDay,@CollectionDate,@qType,@TransType,@LoanPaid,@intPaid", orgIdParameter,officeIdParameter, centerIDParameter, productIDParameter, memberIDParameter, loanTermParameter, collectionDayParameter, collectionDateParameter, qTypeParameter, transTypeParameter, loanPaidParameter, intPaidParameter);

        }


        public int SetSLCTxtKeyPress(int? orgID, int? officeID, int? centerID, int? productID, long? memberID, int? loanTerm, DateTime? collectionDate,  int? transType)
        {
            var OrgIDParaMeter = new SqlParameter("@orgID", orgID);
            var officeIdParameter = new SqlParameter("@officeID", officeID);
            var centerIDParameter = new SqlParameter("@CenterID", centerID);
            var productIDParameter = new SqlParameter("@ProductID", productID);
            var memberIDParameter = new SqlParameter("@MemberID", memberID);
            var loanTermParameter = new SqlParameter("@LoanTerm", loanTerm);
            var collectionDateParameter = new SqlParameter("@CollectionDate", collectionDate);
            var transTypeParameter = new SqlParameter("@TransType", transType);
            return DataContext.Database.ExecuteSqlCommand("SetSLCTxtKeyPress @orgID,@officeID,@CenterID,@ProductID,@MemberID,@LoanTerm,@CollectionDate,@TransType", OrgIDParaMeter, officeIdParameter, centerIDParameter, productIDParameter, memberIDParameter, loanTermParameter, collectionDateParameter, transTypeParameter);

        }


        public IEnumerable<GetSetSLCTxtKeyPress_Result> GetSetSLCTxtKeyPress(int? orgID, int? officeID, int? centerID, int? productID, long? memberID, int? loanTerm, DateTime? collectionDate, int? transType)
        {
            var OrgIDParaMeter = new SqlParameter("@orgID", orgID);
            var officeIdParameter = new SqlParameter("@officeID", officeID);
            var centerIDParameter = new SqlParameter("@CenterID", centerID);
            var productIDParameter = new SqlParameter("@ProductID", productID);
            var memberIDParameter = new SqlParameter("@MemberID", memberID);
            var loanTermParameter = new SqlParameter("@LoanTerm", loanTerm);
            var collectionDateParameter = new SqlParameter("@CollectionDate", collectionDate);
            var transTypeParameter = new SqlParameter("@TransType", transType);
            return DataContext.Database.SqlQuery<GetSetSLCTxtKeyPress_Result>("GetSetSLCTxtKeyPress @orgID,@officeID,@CenterID,@ProductID,@MemberID,@LoanTerm,@CollectionDate,@TransType", OrgIDParaMeter, officeIdParameter, centerIDParameter, productIDParameter, memberIDParameter, loanTermParameter, collectionDateParameter, transTypeParameter);

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
            return DataContext.Database.ExecuteSqlCommand("UpdateSpecialLOan @DailyLoanTrxID,@OfficeId,@CenterId,@MemberID,@ProductID,@LOanterm,@LoanPaid,@IntPaid,@TotalPaid,@TrxType,@orgID", dailyLoanTrxIDParameter, officeIdParameter, CenterIDParameter, MemberIDParameter, ProductIDParameter, LOantermParameter, LoanPaidParameter, IntPaidParameter, TotalPaidParameter, TrxTypeParameter, orgIDParameter);

        }

        public IEnumerable<Proc_get_MaxLoanTerm_Result> Get_MaxLoanTerm(int? orgID, int? officeID, int? centerId, long? memberID, int? productID)
        {
            var orgIDParameter = new SqlParameter("@orgID", orgID);
            var officeIdParameter = new SqlParameter("@OfficeId", officeID);
            var CenterIDParameter = new SqlParameter("@CenterId", centerId);
            var MemberIDParameter = new SqlParameter("@MemberID", memberID);
            var ProductIDParameter = new SqlParameter("@ProductID", productID);
            return DataContext.Database.SqlQuery <Proc_get_MaxLoanTerm_Result>("Proc_get_MaxLoanTerm @orgID,@OfficeId,@CenterId,@MemberID,@ProductID", orgIDParameter, officeIdParameter, CenterIDParameter, MemberIDParameter, ProductIDParameter);

        }


        public IEnumerable<proc_get_SpecialLoanCollection_Result> GetSpecialLoanCollectionDetailReabte(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue)
        {
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@officeId", officeID);
            var vcolday = new SqlParameter("@CollectionDay", vday);
            var filColName = new SqlParameter("@filterColumnName", filterColumnName);
            var filColvalue = new SqlParameter("@filterValue", filterValue);
            return DataContext.Database.SqlQuery<proc_get_SpecialLoanCollection_Result>("proc_get_SpecialLoanCollectionRebate @OrgID,@officeId,@CollectionDay,@filterColumnName,@filterValue", orgIdParameter, officeIdParameter, vcolday, filColName, filColvalue);

        }


        public IEnumerable<proc_get_SpecialLoanCollection_Result> GetSpecialLoanCollectionDetailEmpWise(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue, int? EmpID)
        {
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@officeId", officeID);
            var vcolday = new SqlParameter("@CollectionDay", vday);
            var filColName = new SqlParameter("@filterColumnName", filterColumnName);
            var filColvalue = new SqlParameter("@filterValue", filterValue);
            var EmpIDParameter = new SqlParameter("@EmployeeID", EmpID);
            return DataContext.Database.SqlQuery<proc_get_SpecialLoanCollection_Result>("proc_get_SpecialLoanCollectionEmpWise @OrgID,@officeId,@CollectionDay,@filterColumnName,@filterValue,@EmployeeID", orgIdParameter, officeIdParameter, vcolday, filColName, filColvalue, EmpIDParameter);
   
        }

        public IEnumerable<proc_get_SpecialLoanCollection_Result> GetRebateDetails(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue)
        {
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@officeId", officeID);
            var vcolday = new SqlParameter("@CollectionDay", vday);
            var filColName = new SqlParameter("@filterColumnName", filterColumnName);
            var filColvalue = new SqlParameter("@filterValue", filterValue);
            return DataContext.Database.SqlQuery<proc_get_SpecialLoanCollection_Result>("getRebateDetails @OrgID,@officeId,@CollectionDay,@filterColumnName,@filterValue", orgIdParameter, officeIdParameter, vcolday, filColName, filColvalue);

        }

       
    }
}
