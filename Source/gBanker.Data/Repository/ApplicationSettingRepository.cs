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
    public interface IApplicationSettingRepository : IRepository<ApplicationSetting>
    {
        IEnumerable<DBApplicationSettingsDetail> GetApplicationDetailDetail(int? OrgID,int? officeID);
    }
    public class ApplicationSettingRepository: RepositoryBaseCodeFirst<ApplicationSetting>, IApplicationSettingRepository
    {
        public ApplicationSettingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<DBApplicationSettingsDetail> GetApplicationDetailDetail(int? OrgID, int? officeID)
        {
            var obj = DataContext.ApplicationSettings.Where(x => x.OfficeID == officeID && x.OrgID==OrgID)
             .Select(s => new DBApplicationSettingsDetail()
             {
                
                 OfficeID = s.OfficeID,
                 OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
              
                 ApplicationSettingsID = s.ApplicationSettingsID,
                 BankAccount = s.BankAccount,
                 CashBook = s.CashBook,
                 CellNo = s.CellNo,
                 Email = s.Email,
                 LicenseEndDate = s.LicenseEndDate,
                 LicenseNo = s.LicenseNo,
                 LicenseStartDate = s.LicenseStartDate,
                 MonthClosingDate = s.MonthClosingDate,
                 OperationStartDate = s.OperationStartDate,
                 OrganizationAddress = s.OrganizationAddress,
                 OrganizationID = s.OrganizationID,
                 OrganizationName = s.OrganizationName,
                 PhoneNo = s.PhoneNo,
                 PLAccount = s.PLAccount,
                 ProcessType = s.ProcessType,
                 TransactionDate = s.TransactionDate,
                 YearClosingDate = s.YearClosingDate

             });

            return obj;
        }
    }
}
