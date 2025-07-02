using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface ILoanSummaryRepository : IRepository<LoanSummary>
    {
        //IEnumerable<LoanSummary> GetByProductId(long id, string Code);
        int setWriteOffList(int? OrgID, int? office, long? memberID, int? centerID, int? productID, int? loanTerm, DateTime? trandate, Nullable<decimal> writeOffLOan, Nullable<decimal> writeOffInterest);
        int SetOpeningLoanEntry(int? officeID);
        int MaxLoanTerm(gBanker.Data.CodeFirstMigration.Db.LoanSummary loansummary);
        int MaxLoanTermEdit(gBanker.Data.CodeFirstMigration.Db.LoanSummary loansummary);
        IEnumerable<Proc_get_LoanDisburse_Result> GetLoanDisburse(int? officeID, DateTime? vdate);
        IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetail(int? orgID, int? officeID);
        IEnumerable<DBLoanApproveDetailModel> GetLoanSummaryDetail(int? officeID);
        IEnumerable<DBLoanApproveDetailModel> GetLoanSummaryDetail(int? officeID, string filterColumnName, string filterValue);
        IEnumerable<DBLoanApproveDetailModel> GetLoanSummaryDetailPaged(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount);
        IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetailPaged(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, string jtSorting, out long totalCount, DateTime? transactiondate,int OrgID);

        IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetailPaged(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, string jtSorting, out long totalCount, DateTime? transactiondate, int OrgID,int empid);
        IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetail(int? orgID, int? officeID, string filterColumnName, string filterValue);
        IEnumerable<Proc_get_LoanDisburse_Result> GetLoanDisburse(int? OrgID,int? OfficeId, DateTime? vDate, string filterColumnName, string filterValue);
        IEnumerable<Proc_get_LoanDisburse_Result> GetLoanDisburseSms(int? OrgID, int? OfficeId, DateTime? vDate, string filterColumnName, string filterValue);
        int delWriteOffList(int? OrgID, int? office, long? memberID, int? centerID, int? productID, int? loanTerm, DateTime? trandate, decimal? writeOffLOan, decimal? writeOffInterest);
        int Proc_Set_RepaymentSchedule(Nullable<long> loanSummaryID, Nullable<int> officeID, Nullable<long> memberID, Nullable<short> productID, Nullable<int> centerID, Nullable<byte> memberCategoryID, Nullable<int> loanTerm, Nullable<int> duration, Nullable<System.DateTime> installmentStartDate, string createUser, Nullable<System.DateTime> createDate);
       // int updateDisburseCharge(int? loanSummaryID, int? officeID, int? centerID, int? memberId, int? productID, int? loanterm, decimal? principal, Nullable<System.DateTime> installmentStartDate, Nullable<System.DateTime> disburseDate);
        int InsuranceDailyDisburse(int? officeID, int? centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> loanTerm, Nullable<System.DateTime> disburseDate, Nullable<decimal> principalLoan, Nullable<int> employeeId, Nullable<int> memberCategoryID, Nullable<int> orgID);
        IEnumerable<Proc_get_LoanDisburse_Result> GetPartialLoanDisburse(int? OrgID, int? OfficeId, DateTime? vDate, string filterColumnName, string filterValue);
        IEnumerable<Proc_get_LoanDisburse_Result> GetFirstLoanDisburse(int? OrgID, int? OfficeId, DateTime? vDate, string filterColumnName, string filterValue);
        IEnumerable<Proc_get_LoanDisburse_Result> GetPartialLoanDisburseCCLoan(int? OrgID, int? OfficeId, DateTime? vDate,  string filterColumnName, string filterValue, string filterCCLoan);
    }

    public class LoanSummaryRepository : RepositoryBaseCodeFirst<LoanSummary>, ILoanSummaryRepository
    {
        public LoanSummaryRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public int SetOpeningLoanEntry(int? officeID)
        {
            var officeIdParameter = new SqlParameter("@OfficeId", officeID);
           

            return DataContext.Database.ExecuteSqlCommand("Proc_Set_OpeningLoanEntry @OfficeId", officeIdParameter);
         
        }
        public IEnumerable<Proc_get_LoanDisburse_Result> GetLoanDisburse(int? OfficeId, DateTime? vDate)
        {
            var officeIdParameter = new SqlParameter("@OfficeID", OfficeId);
            var date = new SqlParameter("@date", vDate);
            return DataContext.Database.SqlQuery<Proc_get_LoanDisburse_Result>("Proc_get_LoanDisburse @OfficeID,@date", officeIdParameter, date);
        }
        public IEnumerable<Proc_get_LoanDisburse_Result> GetLoanDisburse(int? OrgID,int? OfficeId, DateTime? vDate, string filterColumnName, string filterValue)
        {
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@OfficeID", OfficeId);
            var date = new SqlParameter("@date", vDate);
            var filColName = new SqlParameter("@filterColumnName", filterColumnName);
            var filColvalue = new SqlParameter("@filterValue", filterValue);
            return DataContext.Database.SqlQuery<Proc_get_LoanDisburse_Result>("Proc_get_LoanDisburse @OrgID,@OfficeID,@date,@filterColumnName,@filterValue", orgIdParameter,officeIdParameter, date, filColName, filColvalue);

        }
        public IEnumerable<Proc_get_LoanDisburse_Result> GetLoanDisburseSms(int? OrgID,int? OfficeId, DateTime? vDate, string filterColumnName, string filterValue)
        {
            var OrgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@OfficeID", OfficeId);
            var date = new SqlParameter("@date", vDate);
            var filColName = new SqlParameter("@filterColumnName", filterColumnName);
            var filColvalue = new SqlParameter("@filterValue", filterValue);
            return DataContext.Database.SqlQuery<Proc_get_LoanDisburse_Result>("Proc_get_LoanDisburse_SMS @OrgID,@OfficeID,@date,@filterColumnName,@filterValue", OrgIdParameter, officeIdParameter, date, filColName, filColvalue);

        }
        public IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetail(int? orgID, int? officeID)
        {

            var obj = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.OrgID==orgID && x.OfficeID == officeID && x.DisburseDate != null)
                .Select(s => new DBLoanApproveDetailModel()
                {
                    //&& x.DisburseDate == null
                    LoanSummaryID = s.LoanSummaryID,
                    OfficeID = s.OfficeID,
                    OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                    OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                    CenterID = s.CenterID,
                    CenterCode = s.Center.CenterCode,
                    CenterName = s.Center.CenterName,
                    MemberID = s.MemberID,
                    MemberCode = s.Member.MemberCode,
                    MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName,
                    GroupID = s.Member.GroupID,
                    ProductCode = s.Product.ProductCode,
                    LoanTerm = s.LoanTerm,
                    PurposeID = s.PurposeID,
                    PrincipalLoan = s.PrincipalLoan,
                    ApproveDate = s.ApproveDate,
                    DisburseDate = s.DisburseDate,
                    Duration = s.Duration,
                    LoanRepaid = s.LoanRepaid,
                    IntCharge = s.IntCharge,
                    IntPaid = s.IntPaid,
                    InstallmentNo = s.InstallmentNo,
                    LoanStatus = s.LoanStatus,
                    Advance = s.Advance,
                    Balance = s.Balance,
                    LoanInstallment = s.LoanInstallment,
                    IntInstallment = s.IntInstallment,
                    InterestRate = s.InterestRate

                }).OrderBy(x => x.CenterCode).ThenBy(x => x.MemberCode);

            return obj;
        }
        public IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetail(int? orgID, int? officeID, string filterColumnName, string filterValue)
        {

            IQueryable<LoanSummary> results = null;
            if (filterColumnName == "MemberCode")
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OrgID==orgID && x.OfficeID == officeID && x.DisburseDate == null && x.Member.MemberCode.Contains(filterValue));
            else if (filterColumnName == "CenterCode")
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OrgID == orgID && x.OfficeID == officeID && x.DisburseDate == null && x.Center.CenterCode.Contains(filterValue));
            else if (filterColumnName == "ProductCode")
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OrgID == orgID && x.OfficeID == officeID && x.DisburseDate == null && x.Product.ProductCode == filterValue);

            else

                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OrgID == orgID && x.OfficeID == officeID && x.DisburseDate == null);

            var obj = results.Select(s => new DBLoanApproveDetailModel()
            {
                    //&& x.DisburseDate == null
                    LoanSummaryID = s.LoanSummaryID,
                    OfficeID = s.OfficeID,
                    OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                    OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                    CenterID = s.CenterID,
                    CenterCode = s.Center.CenterCode,
                    CenterName = s.Center.CenterName,
                    MemberID = s.MemberID,
                    MemberCode = s.Member.MemberCode,
                    MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName,
                    GroupID = s.Member.GroupID,
                    ProductCode = s.Product.ProductCode,
                    LoanTerm = s.LoanTerm,
                    PurposeID = s.PurposeID,
                    PrincipalLoan = s.PrincipalLoan,
                    ApproveDate = s.ApproveDate,
                    DisburseDate = s.DisburseDate,
                    Duration = s.Duration,
                    LoanRepaid = s.LoanRepaid,
                    IntCharge = s.IntCharge,
                    IntPaid = s.IntPaid,
                    InstallmentNo = s.InstallmentNo,
                    LoanStatus = s.LoanStatus,
                    Advance = s.Advance,
                    Balance = s.Balance,
                    LoanInstallment = s.LoanInstallment,
                    IntInstallment = s.IntInstallment,
                    InterestRate = s.InterestRate

                }).OrderBy(x => x.CenterCode).ThenBy(x => x.MemberCode);

            return obj;
        }
        public IEnumerable<DBLoanApproveDetailModel> GetLoanSummaryDetail(int? officeID)
        {
            
            var obj = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0)
                .Select(s => new DBLoanApproveDetailModel()
                {
                    LoanSummaryID = s.LoanSummaryID,
                    OfficeID = s.OfficeID,
                    OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                    OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                    CenterCode = s.Center.CenterCode,
                    MemberCode = s.Member.MemberCode,
                    MemberName = s.Member.FirstName,
                    ProductCode = s.Product.ProductCode,
                    LoanTerm = s.LoanTerm,
                    PurposeID = s.PurposeID,
                    // PurposeCode = s.Purpose.PurposeCode,
                    PrincipalLoan = s.PrincipalLoan,
                    ApproveDate = s.ApproveDate,
                    DisburseDate = s.DisburseDate,
                    Duration = s.Duration,
                    LoanRepaid = s.LoanRepaid,
                    IntCharge = s.IntCharge,
                    IntPaid = s.IntPaid,
                    InstallmentNo = s.InstallmentNo,
                    LoanStatus = s.LoanStatus,
                    Advance = s.Advance,
                    Balance = s.Balance,
                    LoanInstallment = s.LoanInstallment,
                    InstallmentDate = s.InstallmentDate


                }).OrderBy(x => x.CenterCode).ThenBy(x=>x.MemberCode);

            return obj;
        }
        public IEnumerable<DBLoanApproveDetailModel> GetLoanSummaryDetail(int? officeID, string filterColumnName, string filterValue)
        {
            IQueryable<LoanSummary> results = null;
            if (filterColumnName == "MemberCode")
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0 && x.Member.MemberCode.Contains(filterValue));
            else if (filterColumnName == "CenterCode")
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0 && x.Center.CenterCode.Contains(filterValue));
            else if (filterColumnName == "ProductCode")
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0 && x.Product.ProductCode == filterValue);

            else

                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0);
            
            var obj = results.Select(s => new DBLoanApproveDetailModel()
                {
                    LoanSummaryID = s.LoanSummaryID,
                    OfficeID = s.OfficeID,
                    OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                    OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                    CenterCode = s.Center.CenterCode,
                    MemberCode = s.Member.MemberCode,
                    MemberName = s.Member.FirstName,
                    ProductCode = s.Product.ProductCode,
                    LoanTerm = s.LoanTerm,
                    PurposeID = s.PurposeID,
                    // PurposeCode = s.Purpose.PurposeCode,
                    PrincipalLoan = s.PrincipalLoan,
                    ApproveDate = s.ApproveDate,
                    DisburseDate = s.DisburseDate,
                    Duration = s.Duration,
                    LoanRepaid = s.LoanRepaid,
                    IntCharge = s.IntCharge,
                    IntPaid = s.IntPaid,
                    InstallmentNo = s.InstallmentNo,
                    LoanStatus = s.LoanStatus,
                    Advance = s.Advance,
                    Balance = s.Balance,
                    LoanInstallment = s.LoanInstallment,
                    InstallmentDate = s.InstallmentDate


                }).OrderBy(x => x.CenterCode).ThenBy(x => x.MemberCode);

            return obj;
        }
        public IEnumerable<DBLoaneeTransferDetailModel> GetLoaneeTransferDetail()
        {

            var obj = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false)
                .Select(s => new DBLoaneeTransferDetailModel()
                {
                    LoanSummaryID = s.LoanSummaryID,
                    OfficeID = s.OfficeID,
                    OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                    OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                    CenterCode = s.Center.CenterCode,
                    CenterName = s.Center.CenterName,
                    MemberCode = s.Member.MemberCode,
                    MemberName = s.Member.FirstName  + " " + s.Member.LastName                   

                });

            return obj;
        }
        public int MaxLoanTerm(gBanker.Data.CodeFirstMigration.Db.LoanSummary loansummary)
        {
            //var obj = DataContext.LoanSummaries.Where(x => x.OfficeID == loansummary.OfficeID && x.CenterID == loansummary.CenterID && x.MemberID == loansummary.MemberID && x.ProductID == loansummary.ProductID)
            //    .Max(u => (int?)u.LoanTerm) ?? 0;
            var obj = DataContext.LoanSummaries.Where(x => x.OrgID==loansummary.OrgID && x.OfficeID == loansummary.OfficeID && x.MemberID == loansummary.MemberID && x.ProductID == loansummary.ProductID && x.IsActive==true)
              .Max(u => (int?)u.LoanTerm) ?? 0;
            return Convert.ToInt16(obj);
        }

        //public int updateDisburseCharge(long? loanSummaryID, int? officeID, int? centerID, int? memberId, int? productID, int? loanterm, decimal? principal, DateTime? installmentStartDate, DateTime? disburseDate)
        //{
        //    var loanSummaryIDParameter = new SqlParameter("@LoanSummaryID", loanSummaryID);
        //    var officeIdParameter = new SqlParameter("@OfficeID", officeID);
        //    var centerIDParameter = new SqlParameter("@CenterID", centerID);
        //    var MemberIdParameter = new SqlParameter("@MemberId", memberId);
        //    var ProductIDParameter = new SqlParameter("@ProductID", productID);
        //    var LoantermParameter = new SqlParameter("@Loanterm", loanterm);
        //    var PrincipalParameter = new SqlParameter("@Principal", principal);
        //    var installmentStartDateParamenter = new SqlParameter("@installmentStartDate", installmentStartDate);
        //    var DisburseDateParamenter = new SqlParameter("@DisburseDate", disburseDate);
        //    return DataContext.Database.ExecuteSqlCommand("updateDisburseCharge @LoanSummaryID,@OfficeID,@CenterID,@MemberId,@ProductID,@Loanterm,@Principal,@InstallmentStartDate,@DisburseDate", loanSummaryIDParameter, officeIdParameter, centerIDParameter, MemberIdParameter, ProductIDParameter, LoantermParameter, PrincipalParameter, installmentStartDateParamenter, DisburseDateParamenter);
        //}
        public IEnumerable<DBLoanApproveDetailModel> GetLoanSummaryDetailPaged(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount)
        {
            IQueryable<LoanSummary> results = null;
            if (filterColumnName == "MemberCode" && !string.IsNullOrWhiteSpace(filterValue))
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0 && x.Member.MemberCode.Contains(filterValue));
            else if (filterColumnName == "CenterCode" && !string.IsNullOrWhiteSpace(filterValue))
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0 && x.Center.CenterCode.Contains(filterValue));
            else if (filterColumnName == "ProductCode" && !string.IsNullOrWhiteSpace(filterValue))
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0 && x.Product.ProductCode == filterValue);
            else
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0);

            totalCount = results.LongCount();
            var obj = results.OrderBy(x => x.Center.CenterCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBLoanApproveDetailModel()
            {
                LoanSummaryID = s.LoanSummaryID,
                OfficeID = s.OfficeID,
                OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                CenterCode = s.Center.CenterCode,
                MemberCode = s.Member.MemberCode,
                MemberName = s.Member.FirstName,
                ProductCode = s.Product.ProductCode,
                LoanTerm = s.LoanTerm,
                PurposeID = s.PurposeID,              
                PrincipalLoan = s.PrincipalLoan,
                ApproveDate = s.ApproveDate,
                DisburseDate = s.DisburseDate,
                Duration = s.Duration,
                LoanRepaid = s.LoanRepaid,
                IntCharge = s.IntCharge,
                IntPaid = s.IntPaid,
                InstallmentNo = s.InstallmentNo,
                LoanStatus = s.LoanStatus,
                Advance = s.Advance,
                Balance = s.Balance,
                LoanInstallment = s.LoanInstallment,
                IntInstallment=s.IntInstallment,
                InstallmentDate = s.InstallmentDate,
                InstallmentStartDate=s.InstallmentStartDate
            });

            return obj;
        }
        public IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetailPaged(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, string jtSorting, out long totalCount, DateTime? transactiondate,int OrgID)
        {
            ////IQueryable<LoanSummary> results = null;
            ////if (filterColumnName == "MemberCode" && !string.IsNullOrWhiteSpace(filterValue))
            ////    results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.DisburseDate == null && x.ApproveDate==transactiondate &&   (x.TransType == 101 || x.TransType == 102) && x.Member.MemberCode.Contains(filterValue));
            ////else if (filterColumnName == "CenterCode")
            ////    results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.DisburseDate == null && x.ApproveDate == transactiondate && (x.TransType == 101 || x.TransType == 102) && x.Center.CenterCode.Contains(filterValue));
            ////else if (filterColumnName == "ProductCode")
            ////    results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.DisburseDate == null && x.ApproveDate == transactiondate && (x.TransType == 101 || x.TransType == 102) && x.Product.ProductCode == filterValue);

            ////else

            ////    results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.DisburseDate == null && x.ApproveDate == transactiondate && (x.TransType == 101 || x.TransType == 102));
           
            
            IQueryable<LoanSummary> results = null;
            if (filterColumnName == "MemberCode" && !string.IsNullOrWhiteSpace(filterValue))
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.OrgID == OrgID && x.Posted == false && x.OfficeID == officeID && x.DisburseDate == null && x.IsApproved == false && (x.TransType == 101 || x.TransType == 102 || x.TransType == 103) && x.Member.MemberCode.Contains(filterValue));
            else if (filterColumnName == "CenterCode")
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.OrgID == OrgID && x.Posted == false && x.OfficeID == officeID && x.DisburseDate == null && x.IsApproved == false && (x.TransType == 101 || x.TransType == 102 || x.TransType == 103) && x.Center.CenterCode.Contains(filterValue));
            else if (filterColumnName == "ProductCode")
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.OrgID == OrgID && x.Posted == false && x.OfficeID == officeID && x.DisburseDate == null && x.IsApproved == false && (x.TransType == 101 || x.TransType == 102 || x.TransType == 103) && x.Product.ProductCode == filterValue);
            else
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.OrgID == OrgID && x.Posted == false && x.OfficeID == officeID && x.DisburseDate == null && x.IsApproved == false && (x.TransType == 101 || x.TransType == 102 || x.TransType == 103));
           
            totalCount = results.LongCount();
            var obj = results.OrderBy(x => x.Center.CenterCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBLoanApproveDetailModel()
            {
                //&& x.DisburseDate == null
                LoanSummaryID = s.LoanSummaryID,
                OfficeID = s.OfficeID,
                OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                CenterID = s.CenterID,
                CenterCode = s.Center.CenterCode,
                CenterName = s.Center.CenterName,
                MemberID = s.MemberID,
                MemberCode = s.Member.MemberCode + " " + s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + "," + s.Member.RefereeName,
                MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + " " + s.Member.RefereeName,
                GroupID = s.Member.GroupID,
                ProductID=s.ProductID,
                ProductCode = s.Product.ProductCode,
                LoanTerm = s.LoanTerm,
                PurposeID = s.PurposeID,
                PrincipalLoan = s.PrincipalLoan,
                ApproveDate = s.ApproveDate,
                DisburseDate = s.DisburseDate,
                Duration = s.Duration,
                LoanRepaid = s.LoanRepaid,
                IntCharge = s.IntCharge,
                IntPaid = s.IntPaid,
                InstallmentNo = s.InstallmentNo,
                LoanStatus = s.LoanStatus,
                Advance = s.Advance,
                Balance = s.Balance,
                LoanInstallment = s.LoanInstallment,
                IntInstallment = s.IntInstallment,
                InterestRate = s.InterestRate,
                LoanNo = s.LoanNo
            });

          
            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "CenterCode ASC")
                    obj = results.OrderBy(x => x.Center.CenterCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBLoanApproveDetailModel()
                    {
                        //&& x.DisburseDate == null
                        LoanSummaryID = s.LoanSummaryID,
                        OfficeID = s.OfficeID,
                        OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                        OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                        CenterID = s.CenterID,
                        CenterCode = s.Center.CenterCode,
                        CenterName = s.Center.CenterName,
                        MemberID = s.MemberID,
                        MemberCode = s.Member.MemberCode + " " + s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + "," + s.Member.RefereeName,
                        MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName+" "+s.Member.RefereeName,
                        GroupID = s.Member.GroupID,
                        ProductID = s.ProductID,
                        ProductCode = s.Product.ProductCode,
                        LoanTerm = s.LoanTerm,
                        PurposeID = s.PurposeID,
                        PrincipalLoan = s.PrincipalLoan,
                        ApproveDate = s.ApproveDate,
                        DisburseDate = s.DisburseDate,
                        Duration = s.Duration,
                        LoanRepaid = s.LoanRepaid,
                        IntCharge = s.IntCharge,
                        IntPaid = s.IntPaid,
                        InstallmentNo = s.InstallmentNo,
                        LoanStatus = s.LoanStatus,
                        Advance = s.Advance,
                        Balance = s.Balance,
                        LoanInstallment = s.LoanInstallment,
                        IntInstallment = s.IntInstallment,
                        InterestRate = s.InterestRate,
                        LoanNo = s.LoanNo

                    });
                else if (jtSorting == "CenterCode DESC")
                    obj = results.OrderByDescending(x => x.Center.CenterCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBLoanApproveDetailModel()
                    {
                        //&& x.DisburseDate == null
                        LoanSummaryID = s.LoanSummaryID,
                        OfficeID = s.OfficeID,
                        OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                        OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                        CenterID = s.CenterID,
                        CenterCode = s.Center.CenterCode,
                        CenterName = s.Center.CenterName,
                        MemberID = s.MemberID,
                        MemberCode = s.Member.MemberCode + " " + s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + "," + s.Member.RefereeName,
                        MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + " " + s.Member.RefereeName,
                        GroupID = s.Member.GroupID,
                        ProductID = s.ProductID,
                        ProductCode = s.Product.ProductCode,
                        LoanTerm = s.LoanTerm,
                        PurposeID = s.PurposeID,
                        PrincipalLoan = s.PrincipalLoan,
                        ApproveDate = s.ApproveDate,
                        DisburseDate = s.DisburseDate,
                        Duration = s.Duration,
                        LoanRepaid = s.LoanRepaid,
                        IntCharge = s.IntCharge,
                        IntPaid = s.IntPaid,
                        InstallmentNo = s.InstallmentNo,
                        LoanStatus = s.LoanStatus,
                        Advance = s.Advance,
                        Balance = s.Balance,
                        LoanInstallment = s.LoanInstallment,
                        IntInstallment = s.IntInstallment,
                        InterestRate = s.InterestRate,
                        LoanNo = s.LoanNo

                    });
                else if (jtSorting == "MemberCode ASC")
                    obj = results.OrderBy(x => x.Member.MemberCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBLoanApproveDetailModel()
                    {
                        //&& x.DisburseDate == null
                        LoanSummaryID = s.LoanSummaryID,
                        OfficeID = s.OfficeID,
                        OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                        OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                        CenterID = s.CenterID,
                        CenterCode = s.Center.CenterCode,
                        CenterName = s.Center.CenterName,
                        MemberID = s.MemberID,
                        MemberCode = s.Member.MemberCode + " " + s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + "," + s.Member.RefereeName,
                        MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + " " + s.Member.RefereeName,
                        GroupID = s.Member.GroupID,
                        ProductID = s.ProductID,
                        ProductCode = s.Product.ProductCode,
                        LoanTerm = s.LoanTerm,
                        PurposeID = s.PurposeID,
                        PrincipalLoan = s.PrincipalLoan,
                        ApproveDate = s.ApproveDate,
                        DisburseDate = s.DisburseDate,
                        Duration = s.Duration,
                        LoanRepaid = s.LoanRepaid,
                        IntCharge = s.IntCharge,
                        IntPaid = s.IntPaid,
                        InstallmentNo = s.InstallmentNo,
                        LoanStatus = s.LoanStatus,
                        Advance = s.Advance,
                        Balance = s.Balance,
                        LoanInstallment = s.LoanInstallment,
                        IntInstallment = s.IntInstallment,
                        InterestRate = s.InterestRate,
                        LoanNo = s.LoanNo

                    });
                else if (jtSorting == "MemberName ASC")
                    obj = results.OrderBy(x => x.Member.FirstName + " " + x.Member.MiddleName + " " + x.Member.LastName).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBLoanApproveDetailModel()
                    {
                        //&& x.DisburseDate == null
                        LoanSummaryID = s.LoanSummaryID,
                        OfficeID = s.OfficeID,
                        OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                        OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                        CenterID = s.CenterID,
                        CenterCode = s.Center.CenterCode,
                        CenterName = s.Center.CenterName,
                        MemberID = s.MemberID,
                        MemberCode = s.Member.MemberCode + " " + s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + "," + s.Member.RefereeName,
                        MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + " " + s.Member.RefereeName,
                        GroupID = s.Member.GroupID,
                        ProductID = s.ProductID,
                        ProductCode = s.Product.ProductCode,
                        LoanTerm = s.LoanTerm,
                        PurposeID = s.PurposeID,
                        PrincipalLoan = s.PrincipalLoan,
                        ApproveDate = s.ApproveDate,
                        DisburseDate = s.DisburseDate,
                        Duration = s.Duration,
                        LoanRepaid = s.LoanRepaid,
                        IntCharge = s.IntCharge,
                        IntPaid = s.IntPaid,
                        InstallmentNo = s.InstallmentNo,
                        LoanStatus = s.LoanStatus,
                        Advance = s.Advance,
                        Balance = s.Balance,
                        LoanInstallment = s.LoanInstallment,
                        IntInstallment = s.IntInstallment,
                        InterestRate = s.InterestRate,
                        LoanNo = s.LoanNo

                    });
                else if (jtSorting == "ProductCode ASC")
                    obj = results.OrderBy(x => x.Product.ProductCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBLoanApproveDetailModel()
                    {
                        //&& x.DisburseDate == null
                        LoanSummaryID = s.LoanSummaryID,
                        OfficeID = s.OfficeID,
                        OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                        OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                        CenterID = s.CenterID,
                        CenterCode = s.Center.CenterCode,
                        CenterName = s.Center.CenterName,
                        MemberID = s.MemberID,
                        MemberCode = s.Member.MemberCode + " " + s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + "," + s.Member.RefereeName,
                        MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + " " + s.Member.RefereeName,
                        GroupID = s.Member.GroupID,
                        ProductID = s.ProductID,
                        ProductCode = s.Product.ProductCode,
                        LoanTerm = s.LoanTerm,
                        PurposeID = s.PurposeID,
                        PrincipalLoan = s.PrincipalLoan,
                        ApproveDate = s.ApproveDate,
                        DisburseDate = s.DisburseDate,
                        Duration = s.Duration,
                        LoanRepaid = s.LoanRepaid,
                        IntCharge = s.IntCharge,
                        IntPaid = s.IntPaid,
                        InstallmentNo = s.InstallmentNo,
                        LoanStatus = s.LoanStatus,
                        Advance = s.Advance,
                        Balance = s.Balance,
                        LoanInstallment = s.LoanInstallment,
                        IntInstallment = s.IntInstallment,
                        InterestRate = s.InterestRate,
                        LoanNo = s.LoanNo

                    });
                else if (jtSorting == "ProductCode DESC")
                    obj = results.OrderBy(x => x.Product.ProductCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBLoanApproveDetailModel()
                    {
                        //&& x.DisburseDate == null
                        LoanSummaryID = s.LoanSummaryID,
                        OfficeID = s.OfficeID,
                        OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                        OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                        CenterID = s.CenterID,
                        CenterCode = s.Center.CenterCode,
                        CenterName = s.Center.CenterName,
                        MemberID = s.MemberID,
                        MemberCode = s.Member.MemberCode + " " + s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + "," + s.Member.RefereeName,
                        MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + " " + s.Member.RefereeName,
                        GroupID = s.Member.GroupID,
                        ProductID = s.ProductID,
                        ProductCode = s.Product.ProductCode,
                        LoanTerm = s.LoanTerm,
                        PurposeID = s.PurposeID,
                        PrincipalLoan = s.PrincipalLoan,
                        ApproveDate = s.ApproveDate,
                        DisburseDate = s.DisburseDate,
                        Duration = s.Duration,
                        LoanRepaid = s.LoanRepaid,
                        IntCharge = s.IntCharge,
                        IntPaid = s.IntPaid,
                        InstallmentNo = s.InstallmentNo,
                        LoanStatus = s.LoanStatus,
                        Advance = s.Advance,
                        Balance = s.Balance,
                        LoanInstallment = s.LoanInstallment,
                        IntInstallment = s.IntInstallment,
                        InterestRate = s.InterestRate,
                        LoanNo = s.LoanNo

                    });
            }
            else
                obj = results.OrderBy(x => x.Member.MemberCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBLoanApproveDetailModel()
                {
                    //&& x.DisburseDate == null
                    LoanSummaryID = s.LoanSummaryID,
                    OfficeID = s.OfficeID,
                    OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                    OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                    CenterID = s.CenterID,
                    CenterCode = s.Center.CenterCode,
                    CenterName = s.Center.CenterName,
                    MemberID = s.MemberID,
                    MemberCode = s.Member.MemberCode + " " + s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + "," + s.Member.RefereeName,
                    MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + " " + s.Member.RefereeName,
                    GroupID = s.Member.GroupID,
                    ProductID = s.ProductID,
                    ProductCode = s.Product.ProductCode,
                    LoanTerm = s.LoanTerm,
                    PurposeID = s.PurposeID,
                    PrincipalLoan = s.PrincipalLoan,
                    ApproveDate = s.ApproveDate,
                    DisburseDate = s.DisburseDate,
                    Duration = s.Duration,
                    LoanRepaid = s.LoanRepaid,
                    IntCharge = s.IntCharge,
                    IntPaid = s.IntPaid,
                    InstallmentNo = s.InstallmentNo,
                    LoanStatus = s.LoanStatus,
                    Advance = s.Advance,
                    Balance = s.Balance,
                    LoanInstallment = s.LoanInstallment,
                    IntInstallment = s.IntInstallment,
                    InterestRate = s.InterestRate,
                    LoanNo = s.LoanNo

                });
            return obj;
        }
        public int MaxLoanTermEdit(LoanSummary loansummary)
        {

            var obj = DataContext.LoanSummaries.Where(x => x.OrgID==loansummary.OrgID && x.OfficeID == loansummary.OfficeID && x.MemberID == loansummary.MemberID && x.ProductID == loansummary.ProductID && x.DisburseDate==null && x.IsActive == true)
             .Max(u => (int?)u.LoanTerm) ?? 0;
            return Convert.ToInt16(obj);
        }
        public int Proc_Set_RepaymentSchedule(long? loanSummaryID, int? officeID, long? memberID, short? productID, int? centerID, byte? memberCategoryID, int? loanTerm, int? duration, DateTime? installmentStartDate, string createUser, DateTime? createDate)
        {
            var loanSummaryIDParameter = new SqlParameter("@LoanSummaryID", loanSummaryID);
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var memberIDParameter = new SqlParameter("@MemberID", memberID);
            var productIDParameter = new SqlParameter("@ProductID", productID);
            var centerIDParameter = new SqlParameter("@CenterID", centerID);
            var memberCategoryIDParameter = new SqlParameter("@MemberCategoryID", memberCategoryID);
            var loanTermParameter = new SqlParameter("@LoanTerm", loanTerm);
            var durationParameter = new SqlParameter("@Duration", duration);
            var varinstallmentStartDate = new SqlParameter("@InstallmentStartDate", installmentStartDate);
            var varcreateUser = new SqlParameter("@CreateUser", createUser);
            var varcreateDate = new SqlParameter("@CreateDate", createDate);
            return DataContext.Database.ExecuteSqlCommand("Proc_Set_RepaymentSchedule @LoanSummaryID,@OfficeID,@MemberID,@ProductID,@CenterID,@MemberCategoryID,@LoanTerm,@Duration,@InstallmentStartDate,@CreateUser,@createDate", loanSummaryIDParameter, officeIdParameter, memberIDParameter, productIDParameter, centerIDParameter, memberCategoryIDParameter, loanTermParameter, durationParameter, varinstallmentStartDate, varcreateUser, varcreateDate);
            //return DataContext.Database.ExecuteSqlCommand("Proc_Set_RepaymentScheduleWithMonthWise @LoanSummaryID,@OfficeID,@MemberID,@ProductID,@CenterID,@MemberCategoryID,@LoanTerm,@Duration,@InstallmentStartDate,@CreateUser,@createDate", loanSummaryIDParameter, officeIdParameter, memberIDParameter, productIDParameter, centerIDParameter, memberCategoryIDParameter, loanTermParameter, durationParameter, varinstallmentStartDate, varcreateUser, varcreateDate);
        }
        public int setWriteOffList(int? OrgID, int? office, long? memberID, int? centerID, int? productID, int? loanTerm, DateTime? trandate, decimal? writeOffLOan, decimal? writeOffInterest)
        {
            var OrgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@Office", office);
            var memberIDParameter = new SqlParameter("@MemberID", memberID);
            var productIDParameter = new SqlParameter("@ProductID", productID);
            var centerIDParameter = new SqlParameter("@centerID", centerID);
            var loanTermParameter = new SqlParameter("@LoanTerm", loanTerm);
            var TrandateParameter = new SqlParameter("@Trandate", trandate);
            var writeOffLOanParameter = new SqlParameter("@writeOffLOan", writeOffLOan);
            var writeOffInterestParameter = new SqlParameter("@writeOffInterest", writeOffInterest);
            return DataContext.Database.ExecuteSqlCommand("setWriteOffList @OrgID,@Office,@MemberID,@centerID,@productID,@LoanTerm,@Trandate,@writeOffLOan,@writeOffInterest", OrgIdParameter,officeIdParameter, memberIDParameter, centerIDParameter, productIDParameter, loanTermParameter, TrandateParameter, writeOffLOanParameter, writeOffInterestParameter);
        }
        public int delWriteOffList(int? OrgID, int? office, long? memberID, int? centerID, int? productID, int? loanTerm, DateTime? trandate, decimal? writeOffLOan, decimal? writeOffInterest)
        {
            var OrgIDParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@Office", office);
            var memberIDParameter = new SqlParameter("@MemberID", memberID);
            var productIDParameter = new SqlParameter("@ProductID", productID);
            var centerIDParameter = new SqlParameter("@centerID", centerID);
            var loanTermParameter = new SqlParameter("@LoanTerm", loanTerm);
            var TrandateParameter = new SqlParameter("@Trandate", trandate);
            var writeOffLOanParameter = new SqlParameter("@writeOffLOan", writeOffLOan);
            var writeOffInterestParameter = new SqlParameter("@writeOffInterest", writeOffLOan);

            return DataContext.Database.ExecuteSqlCommand("delWriteOffList @OrgID,@Office,@MemberID,@centerID,@productID,@LoanTerm,@Trandate,@writeOffLOan,@writeOffInterest", OrgIDParameter, officeIdParameter, memberIDParameter, centerIDParameter, productIDParameter, loanTermParameter, TrandateParameter, writeOffLOanParameter, writeOffInterestParameter);

        }
        public int InsuranceDailyDisburse(int? officeID, int? centerID, long? memberID, int? productID, int? loanTerm, DateTime? disburseDate, decimal? principalLoan, int? employeeId, int? memberCategoryID, int? orgID)
        {
            var OrgIDParameter = new SqlParameter("@OrgID", orgID);
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var memberIDParameter = new SqlParameter("@MemberID", memberID);
            var productIDParameter = new SqlParameter("@ProductID", productID);
            var centerIDParameter = new SqlParameter("@CenterID", centerID);
            var loanTermParameter = new SqlParameter("@LoanTerm", loanTerm);
            var disburseDateParameter = new SqlParameter("@DisburseDate", disburseDate);
            var PrincipalLoanParameter = new SqlParameter("@PrincipalLoan", principalLoan);
            var EmployeeIdParameter = new SqlParameter("@EmployeeId", employeeId);
            var MemberCategoryIDParameter = new SqlParameter("@MemberCategoryID", memberCategoryID);


            return DataContext.Database.ExecuteSqlCommand("InsuranceDailyDisburse @OfficeID,@CenterID,@MemberID,@ProductID,@LoanTerm,@DisburseDate,@PrincipalLoan,@EmployeeId,@MemberCategoryID,@OrgID", officeIdParameter,centerIDParameter, memberIDParameter, productIDParameter, loanTermParameter, disburseDateParameter, PrincipalLoanParameter, EmployeeIdParameter,MemberCategoryIDParameter,OrgIDParameter);

        }

        //public IEnumerable<LoanSummary> GetByProductId(long id, string Code)
        //{
        //    var lst = DataContext.LoanSummaries.Join(DataContext.Products, acc => acc.ProductID, crt => crt.ProductID,
        //    (acc, crt) => new { acc, crt }).Where(w => w.acc.IsActive == true && w.acc.LoanSummaryID == id && w.crt.MainProductCode == Code);
        //    return lst;
        //}
        public IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetailPaged(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, string jtSorting, out long totalCount, DateTime? transactiondate, int OrgID, int empid)
        {
            IQueryable<LoanSummary> results = null;
            if (filterColumnName == "MemberCode" && !string.IsNullOrWhiteSpace(filterValue))
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.OrgID == OrgID && x.Posted == false && x.OfficeID == officeID && x.DisburseDate == null && x.IsApproved == false && (x.TransType == 101 || x.TransType == 102 || x.TransType == 103) && x.Center.EmployeeId == empid && x.Member.MemberCode.Contains(filterValue));
            else if (filterColumnName == "CenterCode")
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.OrgID == OrgID && x.Posted == false && x.OfficeID == officeID && x.DisburseDate == null && x.IsApproved == false && (x.TransType == 101 || x.TransType == 102 || x.TransType == 103) && x.Center.EmployeeId == empid && x.Center.CenterCode.Contains(filterValue));
            else if (filterColumnName == "ProductCode")
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.OrgID == OrgID && x.Posted == false && x.OfficeID == officeID && x.DisburseDate == null && x.IsApproved == false && (x.TransType == 101 || x.TransType == 102 || x.TransType == 103) && x.Center.EmployeeId == empid && x.Product.ProductCode == filterValue);
            else
                results = DataContext.LoanSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.OrgID == OrgID && x.Posted == false && x.OfficeID == officeID && x.DisburseDate == null && x.IsApproved == false && (x.TransType == 101 || x.TransType == 102 || x.TransType == 102) && x.Center.EmployeeId == empid);

            totalCount = results.LongCount();
            var obj = results.OrderBy(x => x.Center.CenterCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBLoanApproveDetailModel()
            {
                //&& x.DisburseDate == null
                LoanSummaryID = s.LoanSummaryID,
                OfficeID = s.OfficeID,
                OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                CenterID = s.CenterID,
                CenterCode = s.Center.CenterCode,
                CenterName = s.Center.CenterName,
                MemberID = s.MemberID,
                MemberCode = s.Member.MemberCode + " " + s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + "," + s.Member.RefereeName,
                MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + " " + s.Member.RefereeName,
                GroupID = s.Member.GroupID,
                ProductID = s.ProductID,
                ProductCode = s.Product.ProductCode,
                LoanTerm = s.LoanTerm,
                PurposeID = s.PurposeID,
                PrincipalLoan = s.PrincipalLoan,
                ApproveDate = s.ApproveDate,
                DisburseDate = s.DisburseDate,
                Duration = s.Duration,
                LoanRepaid = s.LoanRepaid,
                IntCharge = s.IntCharge,
                IntPaid = s.IntPaid,
                InstallmentNo = s.InstallmentNo,
                LoanStatus = s.LoanStatus,
                Advance = s.Advance,
                Balance = s.Balance,
                LoanInstallment = s.LoanInstallment,
                IntInstallment = s.IntInstallment,
                InterestRate = s.InterestRate,
                LoanNo = s.LoanNo
            });


            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "CenterCode ASC")
                    obj = results.OrderBy(x => x.Center.CenterCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBLoanApproveDetailModel()
                    {
                        //&& x.DisburseDate == null
                        LoanSummaryID = s.LoanSummaryID,
                        OfficeID = s.OfficeID,
                        OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                        OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                        CenterID = s.CenterID,
                        CenterCode = s.Center.CenterCode,
                        CenterName = s.Center.CenterName,
                        MemberID = s.MemberID,
                        MemberCode = s.Member.MemberCode + " " + s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + "," + s.Member.RefereeName,
                        MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + " " + s.Member.RefereeName,
                        GroupID = s.Member.GroupID,
                        ProductID = s.ProductID,
                        ProductCode = s.Product.ProductCode,
                        LoanTerm = s.LoanTerm,
                        PurposeID = s.PurposeID,
                        PrincipalLoan = s.PrincipalLoan,
                        ApproveDate = s.ApproveDate,
                        DisburseDate = s.DisburseDate,
                        Duration = s.Duration,
                        LoanRepaid = s.LoanRepaid,
                        IntCharge = s.IntCharge,
                        IntPaid = s.IntPaid,
                        InstallmentNo = s.InstallmentNo,
                        LoanStatus = s.LoanStatus,
                        Advance = s.Advance,
                        Balance = s.Balance,
                        LoanInstallment = s.LoanInstallment,
                        IntInstallment = s.IntInstallment,
                        InterestRate = s.InterestRate,
                        LoanNo = s.LoanNo

                    });
                else if (jtSorting == "CenterCode DESC")
                    obj = results.OrderByDescending(x => x.Center.CenterCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBLoanApproveDetailModel()
                    {
                        //&& x.DisburseDate == null
                        LoanSummaryID = s.LoanSummaryID,
                        OfficeID = s.OfficeID,
                        OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                        OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                        CenterID = s.CenterID,
                        CenterCode = s.Center.CenterCode,
                        CenterName = s.Center.CenterName,
                        MemberID = s.MemberID,
                        MemberCode = s.Member.MemberCode + " " + s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + "," + s.Member.RefereeName,
                        MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + " " + s.Member.RefereeName,
                        GroupID = s.Member.GroupID,
                        ProductID = s.ProductID,
                        ProductCode = s.Product.ProductCode,
                        LoanTerm = s.LoanTerm,
                        PurposeID = s.PurposeID,
                        PrincipalLoan = s.PrincipalLoan,
                        ApproveDate = s.ApproveDate,
                        DisburseDate = s.DisburseDate,
                        Duration = s.Duration,
                        LoanRepaid = s.LoanRepaid,
                        IntCharge = s.IntCharge,
                        IntPaid = s.IntPaid,
                        InstallmentNo = s.InstallmentNo,
                        LoanStatus = s.LoanStatus,
                        Advance = s.Advance,
                        Balance = s.Balance,
                        LoanInstallment = s.LoanInstallment,
                        IntInstallment = s.IntInstallment,
                        InterestRate = s.InterestRate,
                        LoanNo = s.LoanNo

                    });
                else if (jtSorting == "MemberCode ASC")
                    obj = results.OrderBy(x => x.Member.MemberCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBLoanApproveDetailModel()
                    {
                        //&& x.DisburseDate == null
                        LoanSummaryID = s.LoanSummaryID,
                        OfficeID = s.OfficeID,
                        OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                        OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                        CenterID = s.CenterID,
                        CenterCode = s.Center.CenterCode,
                        CenterName = s.Center.CenterName,
                        MemberID = s.MemberID,
                        MemberCode = s.Member.MemberCode + " " + s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + "," + s.Member.RefereeName,
                        MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + " " + s.Member.RefereeName,
                        GroupID = s.Member.GroupID,
                        ProductID = s.ProductID,
                        ProductCode = s.Product.ProductCode,
                        LoanTerm = s.LoanTerm,
                        PurposeID = s.PurposeID,
                        PrincipalLoan = s.PrincipalLoan,
                        ApproveDate = s.ApproveDate,
                        DisburseDate = s.DisburseDate,
                        Duration = s.Duration,
                        LoanRepaid = s.LoanRepaid,
                        IntCharge = s.IntCharge,
                        IntPaid = s.IntPaid,
                        InstallmentNo = s.InstallmentNo,
                        LoanStatus = s.LoanStatus,
                        Advance = s.Advance,
                        Balance = s.Balance,
                        LoanInstallment = s.LoanInstallment,
                        IntInstallment = s.IntInstallment,
                        InterestRate = s.InterestRate,
                        LoanNo = s.LoanNo

                    });
                else if (jtSorting == "MemberName ASC")
                    obj = results.OrderBy(x => x.Member.FirstName + " " + x.Member.MiddleName + " " + x.Member.LastName).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBLoanApproveDetailModel()
                    {
                        //&& x.DisburseDate == null
                        LoanSummaryID = s.LoanSummaryID,
                        OfficeID = s.OfficeID,
                        OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                        OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                        CenterID = s.CenterID,
                        CenterCode = s.Center.CenterCode,
                        CenterName = s.Center.CenterName,
                        MemberID = s.MemberID,
                        MemberCode = s.Member.MemberCode + " " + s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + "," + s.Member.RefereeName,
                        MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + " " + s.Member.RefereeName,
                        GroupID = s.Member.GroupID,
                        ProductID = s.ProductID,
                        ProductCode = s.Product.ProductCode,
                        LoanTerm = s.LoanTerm,
                        PurposeID = s.PurposeID,
                        PrincipalLoan = s.PrincipalLoan,
                        ApproveDate = s.ApproveDate,
                        DisburseDate = s.DisburseDate,
                        Duration = s.Duration,
                        LoanRepaid = s.LoanRepaid,
                        IntCharge = s.IntCharge,
                        IntPaid = s.IntPaid,
                        InstallmentNo = s.InstallmentNo,
                        LoanStatus = s.LoanStatus,
                        Advance = s.Advance,
                        Balance = s.Balance,
                        LoanInstallment = s.LoanInstallment,
                        IntInstallment = s.IntInstallment,
                        InterestRate = s.InterestRate,
                        LoanNo = s.LoanNo

                    });
                else if (jtSorting == "ProductCode ASC")
                    obj = results.OrderBy(x => x.Product.ProductCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBLoanApproveDetailModel()
                    {
                        //&& x.DisburseDate == null
                        LoanSummaryID = s.LoanSummaryID,
                        OfficeID = s.OfficeID,
                        OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                        OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                        CenterID = s.CenterID,
                        CenterCode = s.Center.CenterCode,
                        CenterName = s.Center.CenterName,
                        MemberID = s.MemberID,
                        MemberCode = s.Member.MemberCode + " " + s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + "," + s.Member.RefereeName,
                        MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + " " + s.Member.RefereeName,
                        GroupID = s.Member.GroupID,
                        ProductID = s.ProductID,
                        ProductCode = s.Product.ProductCode,
                        LoanTerm = s.LoanTerm,
                        PurposeID = s.PurposeID,
                        PrincipalLoan = s.PrincipalLoan,
                        ApproveDate = s.ApproveDate,
                        DisburseDate = s.DisburseDate,
                        Duration = s.Duration,
                        LoanRepaid = s.LoanRepaid,
                        IntCharge = s.IntCharge,
                        IntPaid = s.IntPaid,
                        InstallmentNo = s.InstallmentNo,
                        LoanStatus = s.LoanStatus,
                        Advance = s.Advance,
                        Balance = s.Balance,
                        LoanInstallment = s.LoanInstallment,
                        IntInstallment = s.IntInstallment,
                        InterestRate = s.InterestRate,
                        LoanNo = s.LoanNo

                    });
                else if (jtSorting == "ProductCode DESC")
                    obj = results.OrderBy(x => x.Product.ProductCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBLoanApproveDetailModel()
                    {
                        //&& x.DisburseDate == null
                        LoanSummaryID = s.LoanSummaryID,
                        OfficeID = s.OfficeID,
                        OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                        OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                        CenterID = s.CenterID,
                        CenterCode = s.Center.CenterCode,
                        CenterName = s.Center.CenterName,
                        MemberID = s.MemberID,
                        MemberCode = s.Member.MemberCode + " " + s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + "," + s.Member.RefereeName,
                        MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + " " + s.Member.RefereeName,
                        GroupID = s.Member.GroupID,
                        ProductID = s.ProductID,
                        ProductCode = s.Product.ProductCode,
                        LoanTerm = s.LoanTerm,
                        PurposeID = s.PurposeID,
                        PrincipalLoan = s.PrincipalLoan,
                        ApproveDate = s.ApproveDate,
                        DisburseDate = s.DisburseDate,
                        Duration = s.Duration,
                        LoanRepaid = s.LoanRepaid,
                        IntCharge = s.IntCharge,
                        IntPaid = s.IntPaid,
                        InstallmentNo = s.InstallmentNo,
                        LoanStatus = s.LoanStatus,
                        Advance = s.Advance,
                        Balance = s.Balance,
                        LoanInstallment = s.LoanInstallment,
                        IntInstallment = s.IntInstallment,
                        InterestRate = s.InterestRate,
                        LoanNo = s.LoanNo

                    });
            }
            else
                obj = results.OrderBy(x => x.Member.MemberCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBLoanApproveDetailModel()
                {
                    //&& x.DisburseDate == null
                    LoanSummaryID = s.LoanSummaryID,
                    OfficeID = s.OfficeID,
                    OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                    OfficeName = s.Office == null ? "" : s.Office.OfficeName,
                    CenterID = s.CenterID,
                    CenterCode = s.Center.CenterCode,
                    CenterName = s.Center.CenterName,
                    MemberID = s.MemberID,
                    MemberCode = s.Member.MemberCode + " " + s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + "," + s.Member.RefereeName,
                    MemberName = s.Member.FirstName + " " + s.Member.MiddleName + " " + s.Member.LastName + " " + s.Member.RefereeName,
                    GroupID = s.Member.GroupID,
                    ProductID = s.ProductID,
                    ProductCode = s.Product.ProductCode,
                    LoanTerm = s.LoanTerm,
                    PurposeID = s.PurposeID,
                    PrincipalLoan = s.PrincipalLoan,
                    ApproveDate = s.ApproveDate,
                    DisburseDate = s.DisburseDate,
                    Duration = s.Duration,
                    LoanRepaid = s.LoanRepaid,
                    IntCharge = s.IntCharge,
                    IntPaid = s.IntPaid,
                    InstallmentNo = s.InstallmentNo,
                    LoanStatus = s.LoanStatus,
                    Advance = s.Advance,
                    Balance = s.Balance,
                    LoanInstallment = s.LoanInstallment,
                    IntInstallment = s.IntInstallment,
                    InterestRate = s.InterestRate,
                    LoanNo = s.LoanNo

                });
            return obj;
        }


        public IEnumerable<Proc_get_LoanDisburse_Result> GetPartialLoanDisburse(int? OrgID, int? OfficeId, DateTime? vDate, string filterColumnName, string filterValue)
        {
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@OfficeID", OfficeId);
            var date = new SqlParameter("@date", vDate);
            var filColName = new SqlParameter("@filterColumnName", filterColumnName);
            var filColvalue = new SqlParameter("@filterValue", filterValue);
            return DataContext.Database.SqlQuery<Proc_get_LoanDisburse_Result>("Proc_get_PartialLoanDisburse @OrgID,@OfficeID,@date,@filterColumnName,@filterValue", orgIdParameter, officeIdParameter, date, filColName, filColvalue);

        }
        public IEnumerable<Proc_get_LoanDisburse_Result> GetPartialLoanDisburseCCLoan(int? OrgID, int? OfficeId, DateTime? vDate,  string filterColumnName, string filterValue, string filterCCLoan)
        {
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@OfficeID", OfficeId);
            var date = new SqlParameter("@date", vDate);
            var filColName = new SqlParameter("@filterColumnName", filterColumnName);
            var filColvalue = new SqlParameter("@filterValue", filterValue);
            var qType = new SqlParameter("@Qtype", filterCCLoan);
            return DataContext.Database.SqlQuery<Proc_get_LoanDisburse_Result>("Proc_get_PartialLoanDisburse_CCLoan @OrgID,@OfficeID,@Date,@filterColumnName,@filterValue, @Qtype", orgIdParameter, officeIdParameter, date, filColName, filColvalue, qType);

        }

        public IEnumerable<Proc_get_LoanDisburse_Result> GetFirstLoanDisburse(int? OrgID, int? OfficeId, DateTime? vDate, string filterColumnName, string filterValue)
        {
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@OfficeID", OfficeId);
            var date = new SqlParameter("@date", vDate);
            var filColName = new SqlParameter("@filterColumnName", filterColumnName);
            var filColvalue = new SqlParameter("@filterValue", filterValue);
            return DataContext.Database.SqlQuery<Proc_get_LoanDisburse_Result>("Proc_get_FirstLoanDisburse @OrgID,@OfficeID,@date,@filterColumnName,@filterValue", orgIdParameter, officeIdParameter, date, filColName, filColvalue);

        }
    }
}
