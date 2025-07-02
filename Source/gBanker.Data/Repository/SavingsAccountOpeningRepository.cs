using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface ISavingsAccountOpeningRepository : IRepository<SavingSummary>
    {
        IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetail(int? OrgID,int? officeID);

    }
    public class SavingsAccountOpeningRepository : RepositoryBaseCodeFirst<SavingSummary>, ISavingsAccountOpeningRepository
    {
        public SavingsAccountOpeningRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetail(int? OrgID, int? officeID)
        {
            var obj = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.OrgID==OrgID && x.Posted==false && x.OfficeID == officeID && x.TransType == 2 && x.SavingStatus==1)
               .Select(s => new DBSavingSummaryDetails()
               {
                   SavingSummaryID = s.SavingSummaryID,
                   OfficeID = s.OfficeID,
                   OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                   CenterCode = s.Center.CenterCode,
                   MemberCode = s.Member.MemberCode,
                   MemberName = s.Member.FirstName,
                   ProductCode = s.Product.ProductCode,
                   NoOfAccount = s.NoOfAccount,
                   Deposit = s.Deposit,
                   Withdrawal = s.Withdrawal,
                   SavingInstallment = s.SavingInstallment,
                   InterestRate = s.InterestRate,
                   CumInterest = s.CumInterest,
                   MonthlyInterest = s.MonthlyInterest,
                   Penalty = s.Penalty,
                   OpeningDate = s.OpeningDate,
                   MaturedDate = s.MaturedDate,
                   ClosingDate = s.ClosingDate
                   //,Ref_Employee=s.Ref_EmployeeID

               }).OrderBy(x => x.CenterCode).ThenBy(x => x.MemberCode);

            return obj;
        }
    }
}
